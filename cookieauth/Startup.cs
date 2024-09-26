using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookieauth.Data;
using cookieauth.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace cookieauth
{
    public class Startup
    {
        private IServiceCollection services;
        private ConfigurationManager config;
        public Startup(IServiceCollection _services, ConfigurationManager configuration)
        {
            this.services = _services;
            this.config = configuration;
        }

        public void ConfigureServices()
        {
            services.AddDbContext<AppIdentityDbContext>(option=>{
                option.UseSqlServer(this.config.GetConnectionString("AppIdentity"));
            });

            services.AddIdentity<AppUser, IdentityRole>(options=>{
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

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

            services.AddAuthorization(options=>{
                options.AddPolicy("AdminOnly", policy=> policy.RequireClaim("Admin"));
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