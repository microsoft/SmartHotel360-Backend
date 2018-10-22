using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Bookings.Domain.Booking
{
    public class Booking
    {
        public int Id { get; set; }

        public int IdHotel { get; set; }

        public int IdRoomType { get; set; }

        public string ClientEmail { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public byte NumberOfAdults { get; set; }

        public byte NumberOfChildren { get; set; }

        public byte NumberOfBabies { get; set; }

        public decimal TotalCost { get; set; }
    }
}
