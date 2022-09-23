using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LoanManagementSystem.Areas.LoanMgmt.Controllers
{
    [Area("LoanMgmt")]
    [Authorize]
    public class LoanCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoanCategoriesController> _logger;
        public LoanCategoriesController(ApplicationDbContext context,ILogger<LoanCategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: LoanMgmt/LoanCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoanCategories.ToListAsync());
        }

        // GET: LoanMgmt/LoanCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanCategory = await _context.LoanCategories
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (loanCategory == null)
            {
                return NotFound();
            }

            return View(loanCategory);
        }

        // GET: LoanMgmt/LoanCategories/Create
        [Authorize(Roles = "LoanAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoanMgmt/LoanCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "LoanAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanId,LoanName")] LoanCategory loanCategory)
        {

            loanCategory.LoanName = loanCategory.LoanName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.LoanCategories.Any(l => l.LoanName == loanCategory.LoanName);
            if (duplicateExists)
            {
                ModelState.AddModelError("LoanName", "Duplicate Loan Found!");
            }

            if (ModelState.IsValid)
            {
                _context.LoanCategories.Add(loanCategory);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Created a New Loan: ID = {loanCategory.LoanId} !");
                return RedirectToAction(nameof(Index));
            }

            return View(loanCategory);
        }

        // GET: LoanMgmt/LoanCategories/Edit/5
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanCategory = await _context.LoanCategories.FindAsync(id);
            if (loanCategory == null)
            {
                return NotFound();
            }
            return View(loanCategory);
        }

        // POST: LoanMgmt/LoanCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("LoanId,LoanName")] LoanCategory loanCategory)
        {
            if (id != loanCategory.LoanId)
            {
                return NotFound();
            }
            // Sanitize the data
            loanCategory.LoanName = loanCategory.LoanName.Trim();

            // Validation Checks - Server-side validation
            bool duplicateExists = _context.LoanCategories
                .Any(c => c.LoanName == loanCategory.LoanName && c.LoanId != loanCategory.LoanId);
            if (duplicateExists)
            {
                ModelState.AddModelError("LoanName", "Duplicate Loan Found!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loanCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanCategoryExists(loanCategory.LoanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loanCategory);
        }

        // GET: LoanMgmt/LoanCategories/Delete/5
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanCategory = await _context.LoanCategories
                .FirstOrDefaultAsync(m => m.LoanId == id);
            if (loanCategory == null)
            {
                return NotFound();
            }

            return View(loanCategory);
        }

        // POST: LoanMgmt/LoanCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loanCategory = await _context.LoanCategories.FindAsync(id);
            _context.LoanCategories.Remove(loanCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanCategoryExists(int id)
        {
            return _context.LoanCategories.Any(e => e.LoanId == id);
        }
    }
}
