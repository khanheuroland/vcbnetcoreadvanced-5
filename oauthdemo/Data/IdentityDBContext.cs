using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using oauthdemo.Models;

namespace oauthdemo.Data
{
    public class IdentityDBContext: IdentityDbContext<IdentityUser>
    {
        public IdentityDBContext(DbContextOptions<IdentityDBContext> options):base(options){}
    }

}