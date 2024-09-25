using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jwtauth.common.jwt;
using Microsoft.AspNetCore.Mvc;

namespace jwtauth.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        JwtGenerator jwt;
        public AccountController(JwtGenerator jwt)
        {
            this.jwt = jwt;
        }

        [HttpGet("/token")]
        public IActionResult Token(string email, string password)
        {
            if(email.ToLower()=="khanh.tx@live.com" && password.Trim()=="abc123")
            {
                AuthenticationUser user = new AuthenticationUser(){Email = email, password = password};
                var token = jwt.generateJwtToken(user);
                return Ok(new {token = token});
            }
            else
            {
                return Unauthorized("email");
            }
        }
    }
}