using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using basicauthentication.common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting.Internal;
namespace basicauthentication
{
    public class Startup
    {
        private IServiceCollection services;

        public Startup(IServiceCollection _services)
        {
            this.services = _services;
        }

        public void ConfigureServices()
        {
            this.services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Setup Authentication
            //Basic Authentication use token is compiled <username:password> in base64 
            services.AddAuthentication("BasicAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //Add to authentication
            app.UseAuthorization(); //Default 

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

        }
    }
}