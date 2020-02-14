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
using InGo.Identity;
using Microsoft.AspNetCore.Identity;
using InGo.Services;
using InGo.Models.Data.Links;

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IngoContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly CurrentUser _currentUser;
        private readonly EntityRightsChecker _rightsChecker;

        private IQueryable<Post> FullPosts
        {
            get {
                return _context.Posts
                  .Include(p => p.TagPosts)
                    .ThenInclude(tp => tp.Tag)
                  .Include(p => p.BadgePosts)
                    .ThenInclude(bp => bp.Badge)
                  .Include(p => p.UserPosts)
                  .Include(p => p.Comments).ThenInclude(a => a.Author)
                  .Include(p => p.Author)
                  .Include(p => p.Likes);
            }
        }

        public PostsController(IngoContext context, UserManager<UserIdentity> userManager, CurrentUser currentUser
            , EntityRightsChecker rightsChecker)
        {
            _context = context;
            _userManager = userManager;
            _currentUser = currentUser;
            _rightsChecker = rightsChecker;
        }

        /// <summary>
        /// Get all posts in database.
        /// </summary>
        /// <returns>all posts in database.</returns>
        [HttpGet]
        public async Task<IActionResult> GetNewPosts()
        {
            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            var posts = await FullPosts.ToArrayAsync();
            var result = posts
                .OrderByDescending(p => p.PublishDate)
                .Select(p => p.ViewModel(userId))
                .ToArray();
            return Ok(result);
        }

        /// <summary>
        /// Returns all post sorted by rating.
        /// </summary>
        /// <returns>all post sorted by rating.</returns>
        [HttpGet("top")]
        public async Task<IActionResult> GetTopPosts()
        {
            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            var posts = await FullPosts.ToArrayAsync();
            var result = posts
                .OrderByDescending(p => p.Likes.Count)
                .Select(p => p.ViewModel(userId))
                .ToArray();
            return Ok(result);
        }

        /// <summary>
        /// Returns posts sorted by popularity.
        /// </summary>
        /// <returns>Posts sorted by useful.</returns>
        [HttpGet("useful")]
        public async Task<IActionResult> GetUsefulPosts()
        {
            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            var posts = await FullPosts.ToArrayAsync();
            var result = posts
                .OrderByDescending(p => p.UserPosts.Count)
                .Select(p => p.ViewModel(userId))
                .ToArray();
            return Ok(result);
        }
        
        /// <summary>
        /// Gets all posts with ids from startId to endId.
        /// </summary>
        /// <param name="startId">Minimum id to search.</param>
        /// <param name="endId">Maximum id to search.</param>
        /// <returns>all posts in database.</returns>
        [HttpGet("{startId}/{endId}")]
        public async Task<IActionResult> GetPosts([FromRoute] int startId, [FromRoute] int endId)
        {
            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            if (startId < 1 || endId < 1 || endId < startId)
                return BadRequest("Invalid Id!");

            var result = await FullPosts
                .Where(p => p.Id >= startId && p.Id <= endId)
                .Select(p => p.ViewModel(userId))
                .ToArrayAsync();
            return Ok(result);
        }

        /// <summary>
        /// Searchs post by it's title.
        /// </summary>
        /// <param name="searchItem">Item to search in posts.</param>
        /// <returns>post by it's title.</returns>
        [HttpGet("search/{searchItem}")]
        public async Task<IActionResult> SearchPost([FromRoute] string searchItem)
        {
            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            var result = await FullPosts
                .Where(p => p.Title.Contains(searchItem)
                || p.Content.Contains(searchItem))
                .Select(p => p.ViewModel(userId))
                .ToArrayAsync();
            return Ok(result);
        }

        /// <summary>
        /// Returns post by it's id.
        /// </summary>
        /// <param name="id">Post's id.</param>
        /// <returns>post by it's id.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost([FromRoute] int id)
        {
            var post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
            
            if (post == null)
            {
                return NotFound();
            }

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Adds new post to system.
        /// </summary>
        /// <param name="post">Post to add.</param>
        [HttpPost]
        public async Task<IActionResult> PostPost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await PostExists(post.Id))
                return BadRequest($"Post {post.Id} already exists!");

            post.PublishDate = DateTime.Now;
            post.Author = await _currentUser.GetCurrentUser(HttpContext);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Alteres post.
        /// </summary>
        /// <param name="id">Post's id.</param>
        /// <param name="post">Post's properties.</param>
        [HttpPost("{id}/edit")]
        public async Task<IActionResult> EditPost([FromRoute] int id, [FromBody] Post post)
        {
            post.Id = id;
            if (!await PostExists(id))
                return NotFound($"Post {id} does not exist.");
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to edit this post");

            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to edit this post!");
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Deletes post.
        /// </summary>
        /// <param name="id">Post's id.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost([FromRoute] int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.IsDeleted = true;
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to delete this post.");
            _context.Posts.Update(post);
            foreach (Comment comment in post.Comments)
                comment.IsDeleted = true;
            _context.Comments.UpdateRange(post.Comments);
            foreach (Like like in post.Likes)
                like.IsDeleted = true;
            _context.Likes.UpdateRange(post.Likes);
            foreach (TagPost eachTagPost in post.TagPosts)
                eachTagPost.IsDeleted = true;
            _context.UpdateRange(post.TagPosts);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Adds tag to post.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="tagId">Tag id.</param>
        /// <returns>New post.</returns>
        [HttpPost("{id}/addTag/{tagId}")]
        public async Task<IActionResult> AddTag([FromRoute] int id, [FromRoute] int tagId)
        {
            Post post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            Tag tag = await _context.Tags
                .Where(t => t.Id == tagId)
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");
            if (tag == null)
                return NotFound($"No tag {tagId} found");
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to add tag to this post");
            if (tag.TagPosts.Any(tp => tp.PostId == id))
                return BadRequest($"Post {id} already contains tag {tagId}");

            TagPost tagPost = new TagPost { PostId = id, TagId = tagId };
            _context.Add(tagPost);

            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Adds tags to post.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="tagNames">Tag id.</param>
        /// <returns>New post.</returns>
        [HttpPost("{id}/addTags")]
        public async Task<IActionResult> AddTags([FromRoute] int id, [FromBody] string[] tags)
        {

            Post post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");

            foreach (string tagName in tags)
            {
                Tag tag = await _context.Tags
                .Where(t => t.Name == tagName)
                .FirstOrDefaultAsync();

                if (tag == null)
                    return NotFound($"No tag {tagName} found");

                TagPost tagPost = new TagPost { PostId = id, TagId = tag.Id };
                _context.Add(tagPost);

            }
            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Adds comment to post.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="comment">Comment onject</param>
        /// <returns>New comment</returns>
        [HttpPost("{id}/addComment")]
        public async Task<IActionResult> AddComment([FromRoute] int id, [FromBody] Comment comment)
        {

            Post post = await _context.Posts.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");

            comment.Author = await _currentUser.GetCurrentUser(HttpContext);
            comment.Post = post;
            comment.PublishDate = DateTime.Now;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment.ViewModel);
        }

        /// <summary>
        /// Adds tag by name.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="tagName">Name of tag.</param>
        /// <returns>Post with tag.</returns>
        [HttpPost("{id}/addTag/byname/{tagName}")]
        public async Task<IActionResult> AddTag([FromRoute] int id, [FromRoute] string tagName)
        {
            Post post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            Tag tag = await _context.Tags
                .Where(t => t.Name == tagName)
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");
            if (tag == null)
                return NotFound($"No tag {tagName} found");
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to add tag to this post");
            if (tag.TagPosts.Any(tp => tp.PostId == id))
                return BadRequest($"Post {id} already contains tag {tag.Id}");

            TagPost tagPost = new TagPost { PostId = id, TagId = tag.Id };
            _context.Add(tagPost);

            await _context.SaveChangesAsync();
            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }


        /// <summary>
        /// Removes tag from post.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="tagId">Tag id.</param>
        /// <returns>New post.</returns>
        [HttpPost("{id}/removeTag/{tagId}")]
        public async Task<IActionResult> RemoveTag([FromRoute] int id, [FromRoute] int tagId)
        {
            Post post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            Tag tag = await _context.Tags
                .Where(t => t.Id == tagId)
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");
            if (tag == null)
                return NotFound($"No tag {tagId} found");
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to remove tag from this post");
            if (!tag.TagPosts.Any(tp => tp.PostId == id))
                return BadRequest($"Post {id} does not contain tag {tagId}");

            TagPost tagPost = post.TagPosts.Where(tp => tp.TagId == tagId).FirstOrDefault();
            tagPost.IsDeleted = true;
            _context.Update(tagPost);

            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Removes tag by name.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="tagName">Name of tag.</param>
        /// <returns>Post with tag.</returns>
        [HttpPost("{id}/removeTag/byname/{tagName}")]
        public async Task<IActionResult> RemoveTag([FromRoute] int id, [FromRoute] string tagName)
        {
            Post post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            Tag tag = await _context.Tags
                .Where(t => t.Name == tagName)
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");
            if (tag == null)
                return NotFound($"No tag {tagName} found");
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to remove tag from this post");
            if (!tag.TagPosts.Any(tp => tp.PostId == id))
                return BadRequest($"Post {id} does not contain tag {tag.Id}");

            TagPost tagPost = post.TagPosts.Where(tp => tp.TagId == tag.Id).FirstOrDefault();
            tagPost.IsDeleted = true;
            _context.Update(tagPost);

            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Adds badge to post.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="badgeId">Badge id.</param>
        /// <returns>New post.</returns>
        [HttpPost("{id}/addBadge/{tagId}")]
        public async Task<IActionResult> AddBadge([FromRoute] int id, [FromRoute] int badgeId)
        {
            Post post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            Badge tag = await _context.Badges
                .Where(t => t.Id == badgeId)
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");
            if (tag == null)
                return NotFound($"No tag {badgeId} found");
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to add badge to this post");
            if (tag.BadgePosts.Any(tp => tp.PostId == id))
                return BadRequest($"Post {id} already contains tag {badgeId}");

            BadgePost badgePost = new BadgePost { PostId = id, BadgeId = badgeId };
            _context.Add(badgePost);

            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Removes badge from post.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <param name="badgeId">Badge id.</param>
        /// <returns>New post.</returns>
        [HttpPost("{id}/removeBadge/{tagId}")]
        public async Task<IActionResult> RemoveBadge([FromRoute] int id, [FromRoute] int badgeId)
        {
            Post post = await FullPosts
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            Badge badge = await _context.Badges
                .Where(t => t.Id == badgeId)
                .FirstOrDefaultAsync();

            if (post == null)
                return NotFound($"No post {id} found");
            if (badge == null)
                return NotFound($"No tag {badgeId} found");
            if (!await _rightsChecker.CheckRights(post, HttpContext))
                return BadRequest("No rights to remove badge from this post");
            if (!badge.BadgePosts.Any(tp => tp.PostId == id))
                return BadRequest($"Post {id} does not contain tag {badgeId}");

            BadgePost tagPost = post.BadgePosts.Where(tp => tp.BadgeId == badgeId).FirstOrDefault();
            tagPost.IsDeleted = true;
            _context.Update(tagPost);

            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.CommentsViewModel(userId));
        }

        /// <summary>
        /// Like post.
        /// </summary>
        /// <param name="id">Post id.</param>
        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikePost([FromRoute] int id)
        {
            if (!await PostExists(id))
                return BadRequest($"No post with {id} id.");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            Like like = await _context.Likes.FirstOrDefaultAsync(l => 
                l.UserId == user.Id && l.EntityId == id && l.Type == LikeType.Post );
            
            if (like == null)
            {
                like = new Like { UserId = user.Id, EntityId = id, Type = LikeType.Post };
                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("You have already liked this post");
        }

        /// <summary>
        /// Removes like from the post.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/unLike")]
        public async Task<IActionResult> RemoveLike([FromRoute] int id)
        {
            if (!await PostExists(id))
                return BadRequest($"No post with {id} id.");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            Like like = await _context.Likes.FirstOrDefaultAsync(l =>
                l.UserId == user.Id && l.EntityId == id && l.Type == LikeType.Post);

            if (like == null)
                return BadRequest("You have not liked this post");

            like.IsDeleted = true;
            _context.Likes.Update(like);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        /// <summary>
        /// Saves post to favourites.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <returns>Saved post.</returns>
        [HttpPost("{id}/save")]
        public async Task<IActionResult> SavePost([FromRoute] int id)
        {
            Post post = await FullPosts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return BadRequest($"No post with id={id}!");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            UserPostSave savePost = await _context.UserPostSaves
                .FirstOrDefaultAsync(s => s.UserId == user.Id && s.PostId == post.Id);

            if (savePost != null)
                return BadRequest("Post is already saved!");

            savePost = new UserPostSave { UserId = user.Id, PostId = post.Id };

            _context.UserPostSaves.Add(savePost);
            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.ViewModel(userId));
        }

        /// <summary>
        /// Removes post from saves.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <returns>Unsaved post.</returns>
        [HttpPost("{id}/unsave")]
        public async Task<IActionResult> UnsavePost([FromRoute] int id)
        {
            Post post = await FullPosts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound($"No post with id={id}!");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            UserPostSave savePost = await _context.UserPostSaves.FirstOrDefaultAsync(s => s.PostId == id && s.UserId == user.Id);
            if (savePost == null)
                return BadRequest($"You have not saved this post!");

            savePost.IsDeleted = true;

            _context.UserPostSaves.Update(savePost);
            await _context.SaveChangesAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            return Ok(post.ViewModel(userId));
        }

        /// <summary>
        /// Checks if post was liked.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <returns>Was post liked.</returns>
        [HttpGet("{id}/liked")]
        public async Task<IActionResult> IsLiked([FromRoute] int id)
        {
            Post post = await FullPosts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound($"No post with id = {id}!");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            return Ok(post.Likes.Any(l => l.UserId == user.Id));
        }

        /// <summary>
        /// Checks if post was saved by user.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Was post saved.</returns>
        [HttpGet("{id}/saved")]
        public async Task<IActionResult> IsSaved([FromRoute] int id)
        {
            Post post = await FullPosts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound($"No post with id = {id}!");

            User user = await _currentUser.GetCurrentUser(HttpContext);
            return Ok(post.Likes.Any(l => l.UserId == user.Id));
        }

        /// <summary>
        /// Transforms post to faq.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <returns>New faq.</returns>
        [HttpPost("{id}/toFaq")]
        public async Task<IActionResult> ToFaq([FromRoute] int id)
        {
            Post post = await FullPosts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return NotFound($"No post with id={id}!");

            Faq faq = new Faq
            {
                Question = post.Title,
                Answer = post.Content,
                PublishDate = DateTime.Now
            };

            _context.Posts.Remove(post);
            _context.Faqs.Add(faq);
            await _context.SaveChangesAsync();

            return Ok(faq);
        }

        private Task<bool> PostExists(int id)
        {
            return _context.Posts.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> CheckRights(Post post)
        {
            string identityId = User.Claims.FirstOrDefault().Value;
            User user = await _context.UserProfiles.FirstAsync(u => u.IdentityId == identityId);
            if (post.Author.Id == user.Id)
                return true;
            UserIdentity identity = await _userManager.FindByNameAsync(identityId);
            if (_userManager.GetRolesAsync(identity).Result.Any(r => r == "Admin" || r == "Moderator"))
                return true;
            return false;
        }
    }
}