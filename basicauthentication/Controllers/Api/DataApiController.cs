using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using basicauthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace basicauthentication.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DataApiController : ControllerBase
    {
        public IActionResult Get()
        {
            List<Person> lstPersons = new List<Person>();
            lstPersons.Add(new Person { Id=1, Name = "Nguyen Van Hung", Email="Hung.nguyen@gmail.com"});
            lstPersons.Add(new Person { Id=2, Name = "Nguyen Thu Huong", Email="huong.nguyen@gmail.com"});

            return Ok(lstPersons);
        }
    }
}