using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Authentication.Negotiate;
namespace authenticationdemo
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

            //Windows Authentication
            //Step1: Config service for windows authentication
            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                    .AddNegotiate();

            //Phan quyen
            services.AddAuthorization(options=>
            {
                options.FallbackPolicy = options.DefaultPolicy;
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