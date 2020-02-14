using InGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Services
{
    public class CurrentUser
    {
        private IngoContext _ingoContext;

        public CurrentUser(IngoContext context)
        {
            _ingoContext = context;
        }

        public async Task<User> GetCurrentUser(HttpContext context)
        {
            string userId = context.User.Claims.FirstOrDefault().Value;
            User user = await _ingoContext.UserProfiles.FirstOrDefaultAsync(u => u.IdentityId == userId);

            return user;
        }
    }
}
