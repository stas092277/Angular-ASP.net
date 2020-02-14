using InGo.Identity;
using InGo.Models;
using InGo.Models.Configuration;
using InGo.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Services
{
    /// <summary>
    /// Checks user's rights to modification.
    /// </summary>
    public class EntityRightsChecker
    {
        private readonly CurrentUser _currentUser;
        private readonly UserManager<UserIdentity> _userManager;

        public EntityRightsChecker(CurrentUser currentUser, UserManager<UserIdentity> userManager)
        {
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<bool> CheckRights(IAuthorEntity entity, HttpContext context)
        {
            User user = await _currentUser.GetCurrentUser(context);
            if (user.Id == entity.AuthorId)
                return true;

            var rights = await _userManager.GetRolesAsync(user.Identity);
            if (rights.Contains(Roles.Moderator) || rights.Contains(Roles.Admin))
                return true;

            return false;
        }
    }
}
