using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace jwtauth.common.jwt
{
    public class AuthenticationUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string FullName { get; set;}
    }
    public class JwtGenerator
    {
        public IConfiguration configuration;
        public JwtGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public String generateJwtToken(AuthenticationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"],
                GenerateClaims(user),
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private List<Claim> GenerateClaims(AuthenticationUser user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Email));

            return claims;
        }
    }
}