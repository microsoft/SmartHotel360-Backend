using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Profiles.Data;

namespace SmartHotel.Services.Profiles.Controllers
{
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly ProfilesDbContext _db;
        public ProfilesController(ProfilesDbContext ctx)
        {
            _db = ctx;
        }

        [Route("{id}")]
        [HttpGet]
        public IActionResult Get(string id)
        {
            var profile = _db.Profiles.SingleOrDefault(p => p.UserId == id);
            if (profile == null)
            {
                profile = _db.Profiles.FirstOrDefault(p => p.Alias == id);
            }

            return profile != null ? (IActionResult)Ok(profile) : (IActionResult)NotFound();
        }

    }
}
