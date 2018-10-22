using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Bookings.Queries;

namespace SmartHotel.Services.Bookings.Controllers
{
    [Route("[controller]")]
    public class RoomsController : Controller
    {
        private readonly OccupancyQuery _occupancyQuery;

        public RoomsController(OccupancyQuery occupancyQuery)
        {
            _occupancyQuery = occupancyQuery;
        }

        [HttpGet("{idRoom}/occupancy")]
        public async Task<IActionResult> PredictRoomOcupation(int idRoom, string date)
        {
            if (DateTime.TryParse(date,  System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var dt))
            {
                (double sunny, double notSunny) = await _occupancyQuery.GetRoomOcuppancy(dt, idRoom);
                return Ok(new { OcuppancyIfSunny = sunny, OccupancyIfNotSunny = notSunny });
            }
            else
            {
                return BadRequest("Invalid date " + date);
            }
        }
    }
}