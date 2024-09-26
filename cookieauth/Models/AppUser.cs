using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace cookieauth.Models
{
    public class AppUser:IdentityUser
    {
        public String FirstName{ get; set; }
        public String LastName{ get; set;}
        public String? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime {get;set;}
    }
}