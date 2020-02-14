using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InGo.Models;
using Microsoft.AspNetCore.Authorization;
using InGo.Identity;
using Microsoft.AspNetCore.Identity;
using InGo.Services;

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IngoContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly CurrentUser _currentUser;
        private readonly EntityRightsChecker _rightsChecker;

        private IQueryable<Comment> FullComments
        {
            get
            {
                return _context.Comments.Include(c => c.Post)
                                .Include(c => c.Likes)
                                .Include(c => c.Author)
                                .ThenInclude(u => u.Department);
            }

        }

        public CommentsController(IngoContext context, UserManager<UserIdentity> userManager, CurrentUser currentUser
            , EntityRightsChecker rightsChecker)
        {
            _context = context;
            _userManager = userManager;
            _currentUser = currentUser;
            _rightsChecker = rightsChecker;
        }

        //get all comments
        // GET: api/Comments
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await FullComments.ToArrayAsync();
            var result = comments.Select(c => c.ViewModel);
            return Ok(result);
        }
        
        //get all post comments
        // GET: api/Comments/post/1
        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetCommentsFromPost([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await FullComments.ToArrayAsync();
            var result = comments.Where(c => c.Post.Id == id).Select(c => c.ViewModel);

            return Ok(result);
        }
        
        //get comment by id
        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var comments = await FullComments.ToArrayAsync();
            var result = comments.Where(c => c.Id == id).Select(c => c.ViewModel).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }


            return Ok(result);
        }
        
        //alter comment
        [HttpPost("{id}/edit")]
        public async Task<IActionResult> EditComment([FromRoute] int id, [FromBody] Comment comment)
        {
            if (!await CommentExists(id))
                return NotFound($"Comment {id} does not exist.");
            comment.Id = id;
            if (comment == null)
                return NotFound();
            if (!await _rightsChecker.CheckRights(comment, HttpContext))
                return BadRequest("No rights to edit this comment");
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return NotFound();
            if (!await _rightsChecker.CheckRights(comment, HttpContext))
                return BadRequest("No rights to delete this comment");

            comment.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        /// <summary>
        /// Like the comment.
        /// </summary>
        /// <param name="id">Id of comment.</param>
        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeComment([FromRoute] int id)
        {
            if (!await CommentExists(id))
                return BadRequest($"No comment with {id} id.");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            Like like = await _context.Likes.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.EntityId == id && l.Type == LikeType.Comment);

            if (like != null)
                return BadRequest("You have already liked this comment");

            like = new Like { UserId = user.Id, EntityId = id, Type = LikeType.Comment };
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Removes like from the comment.
        /// </summary>
        /// <param name="id">Id of comment</param>
        /// <returns></returns>
        [HttpPost("{id}/removeLike")]
        public async Task<IActionResult> RemoveLike([FromRoute] int id)
        {
            if (!await CommentExists(id))
                return BadRequest($"No post with {id} id.");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            Like like = await _context.Likes.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.EntityId == id && l.Type == LikeType.Comment);

            if (like == null)
                return BadRequest("You have not liked this post");

            like.IsDeleted = true;
            _context.Likes.Update(like);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private Task<bool> CommentExists(int id)
        {
            return _context.Comments.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> CheckAuthorRights(Comment comment)
        {
            string identityId = User.Claims.FirstOrDefault().Value;
            User user = await _context.UserProfiles.FirstAsync(u => u.IdentityId == identityId);
            if (comment.Author.Id == user.Id)
                return true;
            return false;
        }

        private async Task<bool> CheckRights(Comment comment)
        {
            string identityId = User.Claims.FirstOrDefault().Value;
            User user = await _context.UserProfiles.FirstAsync(u => u.IdentityId == identityId);
            if (comment.Author.Id == user.Id)
                return true;
            UserIdentity identity = await _userManager.FindByNameAsync(identityId);
            if (_userManager.GetRolesAsync(identity).Result.Any(r => r == "Admin" || r == "Moderator"))
                return true;
            return false;
        }
    }
}