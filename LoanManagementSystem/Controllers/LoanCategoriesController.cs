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
    /// <remarks>
    ///     In an ASP.NET Core REST API, there is no need to explicitly check if the model state is Valid. 
    ///     Since the controller class is decorated with the [ApiController] attribute, 
    ///     it takes care of checking if the model state is valid 
    ///     and automatically returns 400 response along the validation errors.
    ///     Example response:
    ///         {
    ///             "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    ///             "title": "One or more validation errors occurred.",
    ///             "status": 400,
    ///             "traceId": "|65b7c07c-4323622998dd3b3a.",
    ///             "errors": {
    ///                 "Email": [
    ///                     "The Email field is required."
    ///                 ],
    ///                 "FirstName": [
    ///                     "The field FirstName must be a string with a minimum length of 2 and a maximum length of 100."
    ///                 ]
    ///             }
    ///         }
    /// </remarks>
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
        public async Task<ActionResult> GetLoanCategory(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var loancategory = await _context.LoanCategories.FindAsync(id);

                if (loancategory == null)
                {
                    return NotFound();
                }

                return Ok(loancategory);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
            //var loanCategory = await _context.LoanCategories.FindAsync(id);

            //if (loanCategory == null)
            //{
            //    return NotFound();
            //}

            //return loanCategory;
        }

        // PUT: api/LoanCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoanCategory(int id, LoanCategory loancategory)
        {
            if (id != loancategory.LoanId)
            {
                return BadRequest();
            }

            // Sanitize the Data
            loancategory.LoanName = loancategory.LoanName.Trim();

            // Server Side Validation
            bool isDuplicateFound = _context.LoanCategories.Any(c => c.LoanName == loancategory.LoanName);
            if (isDuplicateFound)
            {
                ModelState.AddModelError("POST", "Duplicate Loan Category Found!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(loancategory).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException exp)
                {
                    if (!LoanCategoryExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("PUT", exp.Message);
                    }
                }
            }

            return BadRequest(ModelState);
            
        }

        // POST: api/LoanCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostLoanCategory(LoanCategory loancategory)
        {
            // Sanitize the Data
            loancategory.LoanName = loancategory.LoanName.Trim();

            // Server Side Validation
            bool isDuplicateFound = _context.LoanCategories.Any(c => c.LoanName == loancategory.LoanName);
            if (isDuplicateFound)
            {
                ModelState.AddModelError("POST", "Duplicate Loan Category Found!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.LoanCategories.Add(loancategory);
                    await _context.SaveChangesAsync();

                    // return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);

                    // To enforce that the HTTP RESPONSE CODE 201 "CREATED", package the response.
                    var result = CreatedAtAction("GetLoanCategory", new { id = loancategory.LoanId }, loancategory);
                    return Ok(result);
                }
                catch (System.Exception exp)
                {
                    ModelState.AddModelError("POST", exp.Message);
                }
            }

            return BadRequest(ModelState);
            //_context.LoanCategories.Add(loanCategory);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetLoanCategory", new { id = loanCategory.LoanId }, loanCategory);
        }

        // DELETE: api/LoanCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLoanCategory(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            try
            {
                var loancategory = await _context.LoanCategories.FindAsync(id);
                if (loancategory == null)
                {
                    return NotFound();
                }

                _context.LoanCategories.Remove(loancategory);
                await _context.SaveChangesAsync();

                return Ok(loancategory);
            }
            catch (System.Exception exp)
            {
                ModelState.AddModelError("DELETE", exp.Message);
                return BadRequest(ModelState);
            }
        }

        private bool LoanCategoryExists(int id)
        {
            return _context.LoanCategories.Any(e => e.LoanId == id);
        }
        //var loanCategory = await _context.LoanCategories.FindAsync(id);
        //if (loanCategory == null)
        //{
        //    return NotFound();
        //}

        //_context.LoanCategories.Remove(loanCategory);
        //await _context.SaveChangesAsync();
        //return loanCategory;
    }    
}
