using SmartHotel.Services.Hotels.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using static System.Linq.Queryable;

namespace SmartHotel.Services.Hotels.Queries
{
    public class HotelReview
    {
        public string User { get; set; }
        public string Room { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
    }

    public class HotelReviewsQuery
    {
        private readonly HotelsDbContext _db;

        public HotelReviewsQuery(HotelsDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelReview>> Get(int hotelId, int take = 20)
        {
            return await _db
                .Reviews
                .Where(review => review.HotelId == hotelId)
                .Take(take)
                .Select(review => new HotelReview
                {
                    User = review.UserName,
                    Room = review.RoomType,
                    Message = review.Message,
                    Rating = review.Rating,
                    Date = review.Date
                })
                .ToListAsync();
        }
    }
}
