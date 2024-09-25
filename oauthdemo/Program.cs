using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using oauthdemo.Data;
using oauthdemo.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityDBContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdenityDB"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDBContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddCookie(options =>
        {
            options.Cookie.IsEssential = true;
        })
    .AddGoogle(options=>{
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/googlesignin";

        options.SignInScheme = IdentityConstants.ExternalScheme;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<CookiePolicyOptions>(options=>{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
