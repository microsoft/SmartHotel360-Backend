using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Hotels.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Controllers
{
    [Route("[controller]")]
    public class ServicesController : Controller
    {

        private readonly ServicesQuery _svcQuery;

        public ServicesController(ServicesQuery svcQuery)
        {
            _svcQuery = svcQuery;
        }

        [HttpGet("hotel")]
        public async Task<IActionResult> GetHotelServices()
        {
            var data = await _svcQuery.GetAllHotelServices();
            return Ok(data);
        }

        [HttpGet("room")]
        public async Task<IActionResult> GetRoomServices()
        {
            var data = await _svcQuery.GetAllRoomServices();
            return Ok(data);
        }
    }
}
