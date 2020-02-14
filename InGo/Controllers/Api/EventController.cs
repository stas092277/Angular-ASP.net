using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InGo.Models.Links;
using InGo.Models;
using Microsoft.AspNetCore.Authorization;
using InGo.Models.Configuration;

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IngoContext _context;

        public EventController(IngoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all Events
        /// </summary>
        /// <returns>All events availible in db</returns>
        [HttpGet]
        public async Task<IEnumerable<Event>> GetEvent()
        {
            return await _context.Events.ToListAsync();
        }

        /// <summary>
        /// Searching event by title
        /// </summary>
        /// <param name="searchInput">Search input</param>
        /// <returns>Event that includes inputed string</returns>
        [HttpGet("search/{searchInput}")]
        public async Task<IActionResult> SearchEvent([FromRoute] string searchInput)
        {
            var result = await _context.Events.Where(p => p.Title.Contains(searchInput)).ToListAsync();
            if (result == null)
                return NotFound($"Nothing including \"{searchInput}\" is found");
            return Ok(result);
        }

        /// <summary>
        /// Searching event by id
        /// </summary>
        /// <returns>Event with the corresponding id </returns>
        [HttpGet("search/{id}")]
        public async Task<IActionResult> SearchEvent([FromRoute] int id)
        {
            var result = await _context.Events.Where(p => p.Id == id).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get event by id
        /// </summary>
        /// <param name="id">Event id</param>
        /// <returns>Event with specified id</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var myevent = await _context.Events.FindAsync(id);

            if (myevent == null)
            {
                return NotFound();
            }

            return Ok(myevent);
        }

        /// <summary>
        /// Add event
        /// </summary>
        /// <param name="myevent">Event that is added</param>
        /// <returns>New Event</returns>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] Event myevent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (EventExists(myevent.Id))
                return BadRequest($"Event with id {myevent.Id} exists");
            _context.Events.Add(myevent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = myevent.Id }, myevent);
        }


        /// <summary>
        /// Delete event
        /// </summary>
        /// <param name="id">Event id</param>
        //[Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var myevent = await _context.Events.FindAsync(id);
            if (myevent == null)
            {
                return NotFound();
            }

            myevent.IsDeleted = true;
            _context.Events.Update(myevent);
            await _context.SaveChangesAsync();

            return Ok(myevent);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }


}