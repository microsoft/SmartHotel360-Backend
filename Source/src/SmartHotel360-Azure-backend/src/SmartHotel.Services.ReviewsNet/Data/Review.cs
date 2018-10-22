using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.ReviewsNet.Data
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Submitted { get; set; }
        public string Description { get; set; }
        public int HotelId { get; set; }
        public string UserName { get; set; }

        public string FormattedDate { get; set; }
    }
}
