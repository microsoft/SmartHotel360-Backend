using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Services.BookingProcesor.Models
{
    public class BookingRequest
    {
        public int HotelId { get; set; }
        public int UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Babies { get; set; }
        public IEnumerable<RoomBookingRequest> Rooms { get; set; }
        public int Price { get; set; }
    }
}
