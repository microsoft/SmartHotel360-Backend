using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Hotels.Domain.Hotel;
using SmartHotel.Services.Hotels.Queries;

namespace SmartHotel.Services.Hotels.Controllers
{
    [Route("[controller]")]
    public class ConferenceRoomsController : Controller
    {
        private readonly ConferenceRoomSearchQuery _conferenceRoomSearchQuery;

        public ConferenceRoomsController(
            ConferenceRoomSearchQuery conferenceRoomSearchQuery
            )
        {
            _conferenceRoomSearchQuery = conferenceRoomSearchQuery;
        }

        [HttpGet("search")]
        public async Task<ActionResult> Search(int? cityId, int? rating, int? minPrice, int? maxPrice, int? guests)
        {
            var filter = new ConferenceRoomSearchFilter
            {
                CityId = cityId,
                Rating = rating,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Guests = guests
            };
            var conferenceRooms = await _conferenceRoomSearchQuery.Get(filter);
            return Ok(conferenceRooms);
        }
    }
}
