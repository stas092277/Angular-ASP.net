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

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IngoContext _context;

        private IQueryable<Tag> FullTags
        {
            get => _context.Tags
              .Include(t => t.TagPosts)
              .ThenInclude(tp => tp.Post).ThenInclude(tp => tp.Author)
                .Include(t => t.TagPosts)
              .ThenInclude(tp => tp.Post).ThenInclude(tp => tp.Comments);
        }

        public TagsController(IngoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns all tags in service.
        /// </summary>
        /// <returns>all tags in service.</returns>
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            return Ok(await FullTags
                .Select(t => t.EmptyViewModel)
                .ToArrayAsync());
        }

        /// <summary>
        /// Returns tags from startId to endId.
        /// </summary>
        /// <param name="startId">Start id of tag sequence.</param>
        /// <param name="endId">Final id of tag sequence.</param>
        /// <returns>tags from startId to endId.</returns>
        [HttpGet("{startId}/{endId}")]
        public async Task<IActionResult> GetTags([FromRoute] int startId, [FromRoute] int endId)
        {
            if (startId < 1 || endId < 1 || endId < startId)
                return BadRequest("Invalid Id!");

            var result = await FullTags
                .Where(p => p.Id >= startId && p.Id <= endId)
                .Select(p => p.EmptyViewModel)
                .ToArrayAsync();
            return Ok(result);
        }

        /// <summary>
        /// Returns number most popular tags.
        /// </summary>
        /// <param name="number">Number of tags to return.</param>
        /// <returns>number most popular tags.</returns>
        [HttpGet("top/{number}")]
        public async Task<IActionResult> TopTags(int number)
        {
            if (number <= 1)
                return BadRequest("Wrong number!");

            var result = await FullTags
                .OrderByDescending(t => t.TagPosts.Count())
                .Select(t => t.EmptyViewModel)
                .Take(number)
                .ToArrayAsync();
            return Ok(result);
        }

        /// <summary>
        /// Returns tag by id.
        /// </summary>
        /// <param name="id">Id of tag.</param>
        /// <returns>Tag by id.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag([FromRoute] int id)
        {
            var tag = await FullTags
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(tag.ViewModel);
        }

        /// <summary>
        /// Returns tag by name.
        /// </summary>
        /// <param name="name">Name of tag.</param>
        /// <returns>Tag by name.</returns>
        [HttpGet("byname/{name}")]
        public async Task<IActionResult> GetTag([FromRoute] string name)
        {
            var result = await FullTags
                .Where(t => t.Name == name)
                .FirstOrDefaultAsync();

            if (result == null)
                return BadRequest($"No tag called {name}");

            return Ok(result.ViewModel);
        }

        /// <summary>
        /// Searches tag by it's name.
        /// </summary>
        /// <param name="searchTerm">Some title contained in tag's name.</param>
        /// <returns>All tags containing searchTerm.</returns>
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchTag([FromRoute] string searchTerm)
        {
            return Ok(await _context.Tags
                .Where(t => t.Name.Contains(searchTerm))
                .Select(t => t.EmptyViewModel)
                .ToArrayAsync());
        }
   
        /// <summary>
        /// Adds new tag to system.
        /// </summary>
        /// <param name="name">Name of a new tag.</param>
        /// <returns>New tag.</returns>
        [HttpPost("{name}")]
        public async Task<IActionResult> PostTag([FromRoute] string name)
        {
            if (await TagExists(name))
                return BadRequest("Such tag already exists!");

            Tag result = new Tag { Name = name };
            _context.Tags.Add(result);
            await _context.SaveChangesAsync();

            return Ok(result.ViewModel);
        }

        /// <summary>
        /// Adds some one tags to system.
        /// </summary>
        /// <param name="name">Name of a new tag.</param>
        /// <returns>New tag.</returns>
        [HttpPost("addsome")]
        public async Task<IActionResult> AddSometTags([FromBody] string[] tags)
        {
            List<Tag> newTags = new List<Tag>();
            foreach ( string tagName in tags)
            {
                if ( !await TagExists(tagName)) {
                    Tag newTag = new Tag { Name = tagName };
                    newTags.Add(newTag);
                    _context.Tags.Add(newTag);
                }

                await _context.SaveChangesAsync();
            }

            return Ok(newTags);
        }


        /// <summary>
        /// Removes tag from system.
        /// </summary>
        /// <param name="id">Id of tag.</param>
        [HttpDelete("{id}")]
        private async Task<IActionResult> DeleteTag([FromRoute] int id)
        {
            Tag tag = await FullTags
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            tag.IsDeleted = true;
            _context.Tags.Update(tag);
            foreach (TagPost eachTagPost in tag.TagPosts)
                eachTagPost.IsDeleted = true;
            _context.UpdateRange(tag.TagPosts);

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Removes tag from system.
        /// </summary>
        /// <param name="name">Id of tag.</param>
        [HttpDelete("byname/{name}")]
        public async Task<IActionResult> DeleteTag([FromRoute] string name)
        {
            Tag tag = await FullTags
                .Where(t => t.Name == name)
                .FirstOrDefaultAsync();

            tag.IsDeleted = true;
            _context.Tags.Update(tag);
            foreach (TagPost eachTagPost in tag.TagPosts)
                eachTagPost.IsDeleted = true;
            _context.UpdateRange(tag.TagPosts);

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Changes name of tag.
        /// </summary>
        /// <param name="id">Id of tag.</param>
        /// <param name="newName">New name of tag.</param>
        /// <returns>New tag.</returns>
        [HttpPost("update/{id}/{newName}")]
        public async Task<IActionResult> UpdateTag([FromRoute] int id, [FromRoute] string newName)
        {
            if (!await TagExists(id))
                return NotFound($"No tag {id} found");
            if (await TagExists(newName))
                return BadRequest($"Tag {newName} already exists");

            Tag tag = await FullTags
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
            tag.Name = newName;
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return Ok(tag.ViewModel);
        }

        /// <summary>
        /// Changes name of tag.
        /// </summary>
        /// <param name="name">Name of tag.</param>
        /// <param name="newName">New name of tag.</param>
        /// <returns>New tag.</returns>
        [HttpPost("alter/byname/{name}/{newName}")]
        public async Task<IActionResult> AlterTag([FromRoute] string name, [FromRoute] string newName)
        {
            if (!await TagExists(name))
                return NotFound($"No tag {name} found");
            if (await TagExists(newName))
                return BadRequest($"Tag {newName} already exists");

            Tag tag = await FullTags
                .Where(t => t.Name == name)
                .FirstOrDefaultAsync();
            tag.Name = newName;
            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return Ok(tag.ViewModel);
        }

        private Task<bool> TagExists(int id)
        {
            return _context.Tags.AnyAsync(e => e.Id == id);
        }

        private Task<bool> TagExists(string name)
        {
            return _context.Tags.AnyAsync(e => e.Name == name);
        }
    }
}