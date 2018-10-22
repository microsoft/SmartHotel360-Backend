using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Domain.Review
{
    public class Review
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RoomType { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
    }
}
