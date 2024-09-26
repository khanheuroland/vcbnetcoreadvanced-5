using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookieauth.Models
{
    public class RegisterUser
    {
        public String Email { get; set; }
        public String Password { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}