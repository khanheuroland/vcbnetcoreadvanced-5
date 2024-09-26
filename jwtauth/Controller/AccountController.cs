using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using jwtauth.common.jwt;
using jwtauth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwtauth.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        JwtGenerator jwt;
        IConfiguration configuration;
        public AccountController(JwtGenerator jwt, IConfiguration configuration)
        {
            this.jwt = jwt;
            this.configuration = configuration;
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
        [HttpPost]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            /*
            //Get principal from Old Token
            var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            if(principal==null)
            {
                return BadRequest("Invalid Access Token");
            }

            String username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username); //Identity
            if(user==null || user.RefreshToken!= tokenModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid Refresh Token");
            }
            else
            {
                //Generate new token 
                var newToken = jwt.generateJwtToken(user);
                var newRefreshToken = jwt.generateJwtRefreshToken(user);

                user.RefreshToken = newRefreshToken;
                await _userManager.SaveAsync(user); //Update new fresh token to db;
                return new ObjectResult(new {
                    token = newToken,
                    refreshToken = newRefreshToken
                })
            }*/

            return Ok("Pls implement Identity to run");
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(String oldToken)
        {
            var tokenValidationParam = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime=false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))   
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(oldToken, tokenValidationParam, out SecurityToken securityToken);
            if(securityToken is not JwtSecurityToken jwtSecurityToken)
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
        }
    }
}