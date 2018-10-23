using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.ReviewsNet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.ReviewsNet.Query
{
    public class ReviewsQueries
    {
        private readonly ReviewsDbContext _db;

        public ReviewsQueries(ReviewsDbContext db) => _db = db;

        public async Task<IEnumerable<Review>> GetByHotel(int hotelid)
        {
            var result = await _db.Reviews.Where(r => r.HotelId == hotelid)
                .OrderByDescending(r => r.Submitted)
                .ToListAsync();
            return result;
        }

    }
}
