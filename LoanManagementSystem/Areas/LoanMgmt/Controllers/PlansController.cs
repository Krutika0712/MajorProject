using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Areas.LoanMgmt.Controllers
{
    [Area("LoanMgmt")]
    public class PlansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoanMgmt/Plans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Plan.Include(p => p.LoanCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> GetPlanOfCategory(int filterLoanId)
        {
            var viewmodel = await _context.Plan
                                          .Where(m => m.LoanId == filterLoanId)
                                          .Include(m => m.LoanCategory)
                                          .ToListAsync();

            return View(viewName: "Index", model: viewmodel);
        }

        // GET: LoanMgmt/Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan
                .Include(p => p.LoanCategory)
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // GET: LoanMgmt/Plans/Create
        public IActionResult Create()
        {
            ViewData["LoanId"] = new SelectList(_context.LoanCategories, "LoanId", "LoanName");
            return View();
        }

        // POST: LoanMgmt/Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlanId,PlanType,Amount,Description,Eligibility,CreatedDate,LoanId")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoanId"] = new SelectList(_context.LoanCategories, "LoanId", "LoanName", plan.LoanId);
            return View(plan);
        }

        // GET: LoanMgmt/Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["LoanId"] = new SelectList(_context.LoanCategories, "LoanId", "LoanName", plan.LoanId);
            return View(plan);
        }

        // POST: LoanMgmt/Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlanId,PlanType,Amount,Description,Eligibility,CreatedDate,LoanId")] Plan plan)
        {
            if (id != plan.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.PlanId))
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
            ViewData["LoanId"] = new SelectList(_context.LoanCategories, "LoanId", "LoanName", plan.LoanId);
            return View(plan);
        }

        // GET: LoanMgmt/Plans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plan
                .Include(p => p.LoanCategory)
                .FirstOrDefaultAsync(m => m.PlanId == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: LoanMgmt/Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Plan.FindAsync(id);
            _context.Plan.Remove(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Plan.Any(e => e.PlanId == id);
        }
    }
}
