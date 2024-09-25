using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace cookieauth
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
            //Step #1:
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options=>{
                options.AccessDeniedPath = "/acessdenied";
                options.LoginPath = "/Account/Login";
                options.Cookie = new CookieBuilder{
                    Name = ".aspnetcore.vcb.security.cookie",
                    Path = "/",
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
            });
            
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