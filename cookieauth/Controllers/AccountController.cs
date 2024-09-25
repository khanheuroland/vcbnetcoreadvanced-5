using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using cookieauth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace cookieauth.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginUser());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                //Check username and password
                if(user.UserName.ToLower()== "khanh.tx@live.com" && user.Password.Trim() == "abc123")
                {
                    //Create Claims  to store user information (name, email, roles)
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(ClaimTypes.Email, user.UserName));
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
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}