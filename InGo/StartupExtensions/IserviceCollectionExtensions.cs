using InGo.Identity;
using InGo.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace InGo.StartupExtensions
{
    public static class IserviceCollectionExtensions
    {
        public static void AddConfiguredSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                 c =>
                 {
                     c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                     c.IncludeXmlComments(xmlPath);
                 }
                );
        }

        public static void AddConfiguredIdentity(this IServiceCollection services, string connection)
        {
            services.AddDbContext<IngoContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<UserIdentity, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 9;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IngoContext>();
        }

        public static void AddConfiguredJwt(this IServiceCollection services, byte[] key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void AddConfiguredLogging(this IServiceCollection services)
        {
            services.AddLogging(
                builder =>
                {
                    builder.AddFilter("Microsoft", LogLevel.Error)
                           .AddFilter("System", LogLevel.Error)
                           .AddFilter("NToastNotify", LogLevel.Error);
                });
        }
    }
}
