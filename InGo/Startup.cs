using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using InGo.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using InGo.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using InGo.Models.Configuration;
using InGo.StartupExtensions;
using InGo.Services;

namespace InGo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container./
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnectionData");
            services.AddConfiguredIdentity(connection);

            services.AddCors();

            services.AddScoped<CurrentUser>();
            services.AddScoped<EntityRightsChecker>();
            services.AddSingleton<FileName>();
            services.AddScoped<EmailService>();

            services.Configure<JwtSettings>(Configuration.GetSection("JWT"));
            var key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"].ToString());
            services.AddConfiguredJwt(key);

            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            services.AddConfiguredSwagger();

            services.AddConfiguredLogging();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory
            , UserManager<UserIdentity> userManager, IngoContext ingoContext)
        {
            // if (env.IsDevelopment())
            // {
            app.UseDeveloperExceptionPage();
            // }
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}
            app.UseCors(configurePolicy => configurePolicy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()
            .AllowCredentials()
            );


            // loggerFactory.AddFile("logs/log.txt");


            app.UseConfiguredSwagger();

            app.UseAuthentication();

            app.SeedAdmin(userManager, ingoContext);

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.Options.StartupTimeout = new TimeSpan(hours: 0, minutes: 2, seconds: 0);
                    spa.UseAngularCliServer("start");
                }
                else
                {
                    spa.Options.SourcePath = "ClientApp/dist";
                }

            });
        }
    }
}
