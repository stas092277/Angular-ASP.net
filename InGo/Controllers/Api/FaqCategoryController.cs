using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InGo.Models;
using Microsoft.AspNetCore.Authorization;
using InGo.Models.Configuration;

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class FaqCategoryController : ControllerBase
    {
        private readonly IngoContext _context;

        public FaqCategoryController(IngoContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all FAQ categories
        /// </summary>
        /// <returns>All FAQ categories availible in db</returns>
        [HttpGet]
        public async Task<IEnumerable<FaqCategory>> GetFaqCategory()
        {
            return await _context.FaqCategories.ToListAsync();
        }


        /// <summary>
        /// Get FAQ category by id
        /// </summary>
        /// <param name="id">FAQ category id</param>
        /// <returns>FAQ category with specified id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFaqCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var faqCategory = await _context.FaqCategories.FindAsync(id);

            if (faqCategory == null)
            {
                return NotFound();
            }

            return Ok(faqCategory);
        }

        /// <summary>
        /// Get all FAQ posts from category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/faqPosts")]
        public async Task<IActionResult> GetFaqPosrsFromCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.FaqCategories.Include(d => d.Faqs).Where(d => d.Id == id).FirstOrDefaultAsync();

            if (category == null && category.IsDeleted == true)
            {
                return NotFound();
            }

            return Ok(category.Faqs);
        }

        /// <summary>
        /// Delete FAQ category
        /// </summary>
        /// <param name="id">FAQ category id</param>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaqCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var faqCategory = await _context.FaqCategories.FindAsync(id);
            if (faqCategory == null)
            {
                return NotFound();
            }

            faqCategory.IsDeleted = true;
            _context.FaqCategories.Update(faqCategory);
            await _context.SaveChangesAsync();

            return Ok(faqCategory);
        }

        /// <summary>
        /// Add FAQ category
        /// </summary>
        /// <param name="faqCategory"></param>
        /// <returns>New FAQ category</returns>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost]
        public async Task<IActionResult> AddFaqCategory([FromBody] FaqCategory faqCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FaqCategories.Add(faqCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFaqCategory", new { id = faqCategory.Id }, faqCategory);
        }

        private bool FaqCategoryExists(int id)
        {
            return _context.FaqCategories.Any(e => e.Id == id);
        }
    }
}