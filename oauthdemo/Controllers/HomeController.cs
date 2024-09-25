using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using oauthdemo.Models;

namespace oauthdemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private SignInManager<IdentityUser> signInManager;
    public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> _signInManager)
    {
        _logger = logger;
        this.signInManager = _signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("/externallogin")]
    public IActionResult externallogin(string provider)
    {
        var redirectUrl = "http://localhost:8888/googlesignin";
        var properties = this.signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        return new ChallengeResult("Google", properties);
    }

    [AllowAnonymous]
    [HttpGet("/googlesignin")]
    public async Task<IActionResult> GoogleLoginCallback(string returnUrl = null)
    {
        var info = await this.signInManager.GetExternalLoginInfoAsync();

        if(info!=null)
        {
            //Thuc hien sign in voi cookie
            return RedirectToAction("About", "Home");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
