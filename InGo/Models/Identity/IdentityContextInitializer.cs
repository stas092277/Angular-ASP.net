using InGo.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models.Identity
{
    public static class IdentityContextInitializer
    {
        public static void InitializeId(this ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
               //new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() },
               new IdentityRole { Name = "Moderator", NormalizedName = "Moderator".ToUpper() },
               new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() }
               );

            var hasher = new PasswordHasher<UserIdentity>();
            UserIdentity admin = new UserIdentity
            {
                UserName = "Admin",
                NormalizedUserName = "Admin".ToUpper(),
                Email = "Admin",
                NormalizedEmail = "Admin".ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = true,
                SecurityStamp = string.Empty,
            };
            admin.PasswordHash = hasher.HashPassword(admin, "P1234567d");

            builder.Entity<UserIdentity>().HasData(
                admin
            );
        }
    }
}
