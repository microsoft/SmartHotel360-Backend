using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.ReviewsNet.Query;

namespace SmartHotel.Services.ReviewsNet.Controllers
{
    [Route("[controller]")]
    public class ReviewsController : Controller
    {
        private readonly ReviewsQueries _queries;
        private readonly FormatDateService _formatDateSvc;

        public ReviewsController(ReviewsQueries queries, FormatDateService fds)
        {
            _queries = queries;
            _formatDateSvc = fds;
        }

        // GET api/values
        [Route("hotel/{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var reviews= await _queries.GetByHotel(id);

            foreach (var  review in reviews)
            {
                review.FormattedDate = _formatDateSvc.FormatAsString(review.Submitted);
            }

            return Ok(reviews);
        }

    }
}
