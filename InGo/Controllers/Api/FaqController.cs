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
    public class FaqController : ControllerBase
    {
        private readonly IngoContext _context;

        public FaqController(IngoContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all FAQ posts
        /// </summary>
        /// <returns>All FAQ posts availible in db</returns>
        [HttpGet]
        public async Task<IEnumerable<Faq>> GetFaq()
        {
            return await _context.Faqs.ToListAsync();
        }

        /// <summary>
        /// Searching FAQ by title
        /// </summary>
        /// <param name="searchInput">Search input</param>
        /// <returns>FAQ that includes inputed string</returns>
        [HttpGet("search/{searchInput}")]
        public async Task<IActionResult> SearchFAQ([FromRoute] string searchInput)
        {
            var result = await _context.Faqs.Where(p => p.Question.Contains(searchInput)).ToListAsync();
            if (result == null)
                return NotFound($"Nothing including \"{searchInput}\" is found");
            return Ok(result);
        }

        /// <summary>
        /// Searching FAQ by id
        /// </summary>
        /// <returns>FAQ with the corresponding id </returns>
        [HttpGet("search/{id}")]
        public async Task<IActionResult> SearchFAQ([FromRoute] int id)
        {
            var result = await _context.Faqs.Where(p => p.Id == id).ToListAsync();
            return Ok(result);
        }


        /// <summary>
        /// Get FAQ by id
        /// </summary>
        /// <param name="id">FAQ id</param>
        /// <returns>FAQ with specified id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFaq([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var faq = await _context.Faqs.FindAsync(id);

            if (faq == null)
            {
                return NotFound();
            }

            return Ok(faq);
        }

        /// <summary>
        /// Add FAQ (out of post)
        /// </summary>
        /// <param name="faq">FAQ that is added</param>
        /// <returns>New FAQ</returns>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost]
        public async Task<IActionResult> PostFaq([FromBody] Faq faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (FaqExists(faq.Id))
                return BadRequest($"FAQ post with id {faq.Id} exists");
            faq.PublishDate = DateTime.Now;
            _context.Faqs.Add(faq);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFaq", new { id = faq.Id }, faq);
        }

        /// <summary>
        /// Delete FAQ
        /// </summary>
        /// <param name="id">FAQ id</param>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFaq([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }

            faq.IsDeleted = true;
            _context.Faqs.Update(faq);
            await _context.SaveChangesAsync();

            return Ok(faq);
        }

        private bool FaqExists(int id)
        {
            return _context.Faqs.Any(e => e.Id == id);
        }
    }
}