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

namespace LoanManagementSystem.Areas.LoanMgmt.Controllers
{
    [Area("LoanMgmt")]
    [Authorize]
    public class ApprovalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApprovalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoanMgmt/Approvals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Approval.Include(a => a.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LoanMgmt/Approvals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approval = await _context.Approval
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(m => m.ApprovalId == id);
            if (approval == null)
            {
                return NotFound();
            }

            return View(approval);
        }

        // GET: LoanMgmt/Approvals/Create
        [Authorize(Roles = "LoanAdmin")]
        public IActionResult Create()
        {
            //var Customers = _context.Customer.Where(c => c.CustomerId == id);
            //ViewBag.CustomerId = Customers.CustomerId;
            //ViewBag.FullName = Customers.FullName;
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName");
            return View();
        }

        // POST: LoanMgmt/Approvals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> Create([Bind("ApprovalId,Status,ApprovalDate,CustomerId")] Approval approval)
        {
            if (ModelState.IsValid)
            {
                _context.Add(approval);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", approval.CustomerId);
            return View(approval);
        }

        // GET: LoanMgmt/Approvals/Edit/5
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approval = await _context.Approval.FindAsync(id);
            if (approval == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", approval.CustomerId);
            return View(approval);
        }

        // POST: LoanMgmt/Approvals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("ApprovalId,Status,ApprovalDate,CustomerId")] Approval approval)
        {
            if (id != approval.ApprovalId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(approval);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApprovalExists(approval.ApprovalId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "FullName", approval.CustomerId);
            return View(approval);
        }

        // GET: LoanMgmt/Approvals/Delete/5
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var approval = await _context.Approval
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(m => m.ApprovalId == id);
            if (approval == null)
            {
                return NotFound();
            }

            return View(approval);
        }

        // POST: LoanMgmt/Approvals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "LoanAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var approval = await _context.Approval.FindAsync(id);
            _context.Approval.Remove(approval);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApprovalExists(int id)
        {
            return _context.Approval.Any(e => e.ApprovalId == id);
        }
    }
}
