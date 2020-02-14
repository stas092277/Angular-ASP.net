using InGo.Identity;
using InGo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.StartupExtensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseConfiguredSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "swagger";
                }
                );
        }

        public static void SeedAdmin(this IApplicationBuilder app, UserManager<UserIdentity> userManager, IngoContext ingoContext)
        {
            var admin = ingoContext.UserProfiles.Find(1);
            var adminIdentity = userManager.FindByNameAsync("Admin").Result;
            if (admin.IdentityId == adminIdentity?.Id)
                return;
            admin.Identity = adminIdentity;
            ingoContext.UserProfiles.Update(admin);
            ingoContext.SaveChanges();
        }
    }
}
