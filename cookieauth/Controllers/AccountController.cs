using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using cookieauth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cookieauth.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginUser());
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser user)
        {
            AppUser appUser = Activator.CreateInstance<AppUser>(); //Create user Identity
            appUser.UserName = user.Email;
            appUser.FirstName = user.FirstName;
            appUser.LastName = user.LastName;

            var result = await _userManager.CreateAsync(appUser, user.Password);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, true);
                //Check username and password
                if(result.Succeeded)
                {
                    //Create Claims  to store user information (name, email, roles)
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(ClaimTypes.Email, user.UserName));
                    claims.Add(new Claim("Admin", "true"));
                    //Create Identity 
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "cookie");
                    //Create Pricipal
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    //Thuc hien cookie authen
                    await HttpContext.SignInAsync(
                        principal: principal, 
                        properties: new AuthenticationProperties{
                            IsPersistent=true //Remember
                        }
                    );

                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            //await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet("/acessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}