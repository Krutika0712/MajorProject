using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanManagementSystem.Data;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class LoanCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoanCategoriesController> _logger;
        public LoanCategoriesController(ApplicationDbContext context , ILogger<LoanCategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/LoanCategories
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<LoanCategory>>> GetLoanCategories()
        public async Task<IActionResult> GetLoanCategories()
        {
            try
            {
                var categories = await _context.LoanCategories
                                        .Include(c => c.Plans)
                                        .ToListAsync();
                //Check if data exists in the Database
                if (categories == null)
                {
                    return NotFound();          // RETURN: No data was found            HTTP 404
                }
                return Ok(categories);          // RETURN: OkObjectResult - good result HTTP 200
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message); // RETURN: BadResult                    HTTP 400
            }
                       
        }

        // GET: api/LoanCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanCategory>> GetLoanCategory(int id)
        {
            var loanCategory = await _context.LoanCategories.FindAsync(id);

            if (loanCategory == null)
            {
                return NotFound();
            }

            return loanCategory;
        }

        // PUT: api/LoanCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanCategory(int id, LoanCategory loanCategory)
        {
            if (id != loanCategory.LoanId)
            {
                return BadRequest();
            }

            _context.Entry(loanCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LoanCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LoanCategory>> PostLoanCategory(LoanCategory loanCategory)
        {
            _context.LoanCategories.Add(loanCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoanCategory", new { id = loanCategory.LoanId }, loanCategory);
        }

        // DELETE: api/LoanCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LoanCategory>> DeleteLoanCategory(int id)
        {
            var loanCategory = await _context.LoanCategories.FindAsync(id);
            if (loanCategory == null)
            {
                return NotFound();
            }

            _context.LoanCategories.Remove(loanCategory);
            await _context.SaveChangesAsync();

            return loanCategory;
        }

        private bool LoanCategoryExists(int id)
        {
            return _context.LoanCategories.Any(e => e.LoanId == id);
        }
    }
}
