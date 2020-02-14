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
    [Authorize]
    [ApiController]
    public class EventCategoryController : ControllerBase
    {
        private readonly IngoContext _context;

        public EventCategoryController(IngoContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all event categories
        /// </summary>
        /// <returns>All event categories availible in db</returns>
        [HttpGet]
        public async Task<IEnumerable<EventCategory>> GetEventCategory()
        {
            return await _context.EventCategories.ToListAsync();
        }


        /// <summary>
        /// Get Event category by id
        /// </summary>
        /// <param name="id">Event category id</param>
        /// <returns>Event category with specified id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventCategory = await _context.EventCategories.FindAsync(id);

            if (eventCategory == null)
            {
                return NotFound();
            }

            return Ok(eventCategory);
        }

        /// <summary>
        /// Get all events from category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/events")]
        public async Task<IActionResult> GetEventsFromCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.EventCategories.Include(d => d.Events).Where(d => d.Id == id).FirstOrDefaultAsync();

            if (category == null && category.IsDeleted == true)
            {
                return NotFound();
            }

            return Ok(category.Events);
        }

        /// <summary>
        /// Delete Event category
        /// </summary>
        /// <param name="id">Event category id</param>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        public async Task<IActionResult> DeleteEventCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventCategory = await _context.EventCategories.FindAsync(id);
            if (eventCategory == null)
            {
                return NotFound();
            }

            eventCategory.IsDeleted = true;
            _context.EventCategories.Update(eventCategory);
            await _context.SaveChangesAsync();

            return Ok(eventCategory);
        }

        /// <summary>
        /// Add event category
        /// </summary>
        /// <param name="eventCategory"></param>
        /// <returns>New event category</returns>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost]
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        public async Task<IActionResult> AddEventCategory([FromBody] EventCategory eventCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EventCategories.Add(eventCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventCategory", new { id = eventCategory.Id }, eventCategory);
        }

        private bool EventCategoryExists(int id)
        {
            return _context.EventCategories.Any(e => e.Id == id);
        }
    }
}