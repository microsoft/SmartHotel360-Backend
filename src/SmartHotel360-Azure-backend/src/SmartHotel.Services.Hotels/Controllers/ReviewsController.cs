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
    public class ReviewsController : Controller
    {
        private readonly HotelReviewsQuery _hotelReviewsQuery;

        public ReviewsController(HotelReviewsQuery hotelReviewsQuery)
        {
            _hotelReviewsQuery = hotelReviewsQuery;
        }

        [HttpGet("{hotelId:int}")]
        public async Task<ActionResult> Get(int hotelId)
        {
            var reviews = await _hotelReviewsQuery.Get(hotelId);
            return Ok(reviews);
        }
    }
}
