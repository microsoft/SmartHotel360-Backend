using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Hotels.Domain.Hotel;
using SmartHotel.Services.Hotels.Queries;
using Microsoft.AspNetCore.Authorization;

namespace SmartHotel.Services.Hotels.Controllers
{
    [Route("[controller]")]
    public class FeaturedController : Controller
    {
        private readonly FeaturedItemsHotelsQuery _featuredQuery;

        public FeaturedController(
            FeaturedItemsHotelsQuery featuredQuery
            )
        {
            _featuredQuery = featuredQuery;
        }
        
        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            var userid = (string)null;
            if (User.Identity.IsAuthenticated)
            {
                userid = User.Claims.First(c => c.Type == "emails").Value;
            }

            
            var hotels = userid != null ? await _featuredQuery.GetForUser(userid)  : await _featuredQuery.Get();
            return Ok(hotels);
        }
    }
}
