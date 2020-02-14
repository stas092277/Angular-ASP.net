using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InGo.Models;
using InGo.Models.Links;
using Microsoft.AspNetCore.Authorization;
using InGo.Models.Configuration;

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BadgesController : ControllerBase
    {
        private readonly IngoContext _context;

        public BadgesController(IngoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all badges in system.
        /// </summary>
        /// <returns>all badges in system.</returns>
        [HttpGet]
        public async Task<IEnumerable<Badge>> GetBadges()
        {
            return await _context.Badges.ToArrayAsync();
        }

        /// <summary>
        /// Returns badge by id.
        /// </summary>
        /// <param name="id">Id of badge.</param>
        /// <returns>Badge by id.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBadge([FromRoute] int id)
        {
            var badge = await _context.Badges.FindAsync(id);

            if (badge == null)
            {
                return NotFound();
            }

            return Ok(badge);
        }

        /// <summary>
        /// Adds new badge to system.
        /// </summary>
        /// <param name="badge"></param>
        /// <returns></returns>
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost]
        public async Task<IActionResult> PostBadge([FromBody] Badge badge)
        {
            _context.Badges.Add(badge);
            await _context.SaveChangesAsync();

            return Ok(badge);
        }

        /// <summary>
        /// Removes badge from system.
        /// </summary>
        /// <param name="id">Id of badge.</param>
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBadge([FromRoute] int id)
        {
            var badge = await _context.Badges.FindAsync(id);
            if (badge == null)
            {
                return NotFound();
            }

            badge.IsDeleted = true;
            _context.Badges.Update(badge);
            foreach (BadgePost eachBadgePost in badge.BadgePosts)
                eachBadgePost.IsDeleted = true;
            _context.UpdateRange(badge.BadgePosts);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Updates badge content.
        /// </summary>
        /// <param name="id">Id of badge.</param>
        /// <param name="badge">New Badge content.</param>
        /// <returns>New badge content.</returns>
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> EditBadge([FromRoute] int id, [FromBody] Badge badge)
        {
            if (!await BadgeExists(id))
                return NotFound($"No badge {id} found");

            badge.Id = id;
            _context.Badges.Update(badge);
            await _context.SaveChangesAsync();

            return Ok(badge);
        }

        private Task<bool> BadgeExists(int id)
        {
            return _context.Badges.AnyAsync(e => e.Id == id);
        }
    }
}