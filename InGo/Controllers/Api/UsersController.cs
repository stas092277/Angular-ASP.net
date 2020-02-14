using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InGo.Models;
using Microsoft.AspNetCore.Authorization;
using InGo.Services;
using InGo.Models.Configuration;
using InGo.Identity;
using Microsoft.AspNetCore.Identity;

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IngoContext _context;
        private readonly CurrentUser _currentUser;
        private readonly UserManager<UserIdentity> _userManager;

        public UsersController(IngoContext context, CurrentUser currentUser, UserManager<UserIdentity> userManager)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
        }

        private IQueryable<User> FullUsers
        {
            get {
                return _context.UserProfiles.Include(u => u.Posts)
                                .Include(u => u.Badges)
                                .Include(u => u.EventUsers)
                                .Include(u => u.Department);
            }

        }

        //get all users
        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await FullUsers.ToArrayAsync();
            return Ok(users.Select(u => u.ViewModel));
        }

        //get user by id
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = await FullUsers.ToArrayAsync();
            var result = user.Where(u => u.Id == id).Select(u => u.ViewModel).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }


            return Ok(result);
        }

        //get all users startId ID1 to endId
        [HttpGet("{startId}/{endId}")]
        public async Task<IActionResult> GetUsers([FromRoute] int startId, [FromRoute] int endId)
        {
            if (startId < 1 || endId < 1 || endId < startId)
                return BadRequest("Invalid Id!");
            var users = await FullUsers.ToArrayAsync();
            var result = users
                .Where(u => u.Id >= startId && u.Id <= endId)
                .Select(u => u.ViewModel);
            return Ok(result);
        }

        //search user by email
        [HttpGet("{id}/email")]
        public async Task<IActionResult> SearchUser([FromRoute] string searchTerm)
        {
            var result = await FullUsers
                .Where(u => u.About.Contains(searchTerm)
                || u.FirstName.Contains(searchTerm)
                || u.LastName.Contains(searchTerm))
                .Select(u => u.ViewModel)
                .ToArrayAsync();
            return Ok(result);
        }

        //update user
        [HttpPost("{id}/update")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await UserExists(id))
                return NotFound($"User {id} does not exist.");

            User candid = await _currentUser.GetCurrentUser(HttpContext);
            if (user.Id != candid.Id)
                return BadRequest("No rights to edit this user.");

            //user.Id = id;
            candid.About = user.About;
            candid.Email = user.Email;
            candid.FirstName = user.LastName;
            candid.LastName = user.LastName;
            candid.ImgUrl = user.ImgUrl;
            candid.Type = user.Type;

            _context.UserProfiles.Update(candid);
            await _context.SaveChangesAsync();

            return Ok(candid);
        }

        //delete user
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.UserProfiles.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            User currentUser = await _currentUser.GetCurrentUser(HttpContext);
            if (id != currentUser.Id || !await _userManager.IsInRoleAsync(currentUser.Identity, Roles.ModeratorOrAdmin))
                return BadRequest("No rights to edit this user.");

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>
        /// Retrieves user's posts.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <returns></returns>
        [HttpGet("{id}/posts")]
        public async Task<IActionResult> GetPosts([FromRoute] int id)
        {
            if (!await UserExists(id))
                return BadRequest($"User {id} does not exist.");

            User user = await FullUsers.FirstOrDefaultAsync(u => u.Id == id);
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.BadgePosts)
                    .ThenInclude(bp => bp.Badge)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.TagPosts)
                    .ThenInclude(tp => tp.Tag)
                .Where(p => p.AuthorId == id)
                .ToListAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            var result = posts.Select(p => p.ViewModel(userId));

            return Ok(result);
        }

        /// <summary>
        /// Grants user moderator rights.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>New moderator.</returns>
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost("{id}/grantModerator")]
        public async Task<IActionResult> GrantModeratorRights([FromRoute] int id)
        {
            User user = await FullUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound($"No user with id={id}!");
            UserIdentity identity = user.Identity;
            await _userManager.AddToRoleAsync(identity, Roles.Moderator);

            return Ok(user.ViewModel);
        }

        /// <summary>
        /// Restricts moderator rights from user.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Poor user with no rights.</returns>
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        [HttpPost("{id}/restrictModerator")]
        public async Task<IActionResult> RestrictModeratorRights([FromRoute] int id)
        {
            User user = await FullUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound($"No user with id={id}!");
            UserIdentity identity = user.Identity;
            if (await _userManager.IsInRoleAsync(identity, Roles.Admin))
                return BadRequest($"No rights to modify admin!");

            await _userManager.RemoveFromRoleAsync(identity, Roles.Moderator);

            return Ok(user.ViewModel);
        }

        /// <summary>
        /// Returns saved posts.
        /// </summary>
        /// <param name="id">USer id.</param>
        /// <returns>Saved posts.</returns>
        [HttpGet("{id}/savedPosts")]
        public async Task<IActionResult> GetSavedPosts([FromRoute] int id)
        {
            if (!await UserExists(id))
                return BadRequest($"User {id} does not exist.");

            User user = await FullUsers.FirstOrDefaultAsync(u => u.Id == id);
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.BadgePosts)
                    .ThenInclude(bp => bp.Badge)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Include(p => p.TagPosts)
                    .ThenInclude(tp => tp.Tag)
                .Include(p => p.UserPosts)
                .Where(p => p.UserPosts.Any(up => up.UserId == id))
                .ToListAsync();

            int userId = (await _currentUser.GetCurrentUser(HttpContext)).Id;
            var result = posts
                .Where(p => p.UserPosts.Any(up => up.UserId == id))
                .Select(p => p.ViewModel(userId));

            return Ok(result);
        }

        private Task<bool> UserExists(int id)
          {
              return _context.UserProfiles.AnyAsync(e => e.Id == id && e.IsDeleted == false);
          }
      }
  }
 