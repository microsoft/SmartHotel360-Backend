using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Services.BookingProcesor.Models
{
    public class RoomBookingRequest
    {
        public int RoomType { get; set; }
        public int Quantity { get; set; }
    }
}
