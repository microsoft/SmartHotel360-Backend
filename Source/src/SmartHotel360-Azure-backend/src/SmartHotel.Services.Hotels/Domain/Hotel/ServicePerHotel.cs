using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Domain.Hotel
{
    public class ServicePerHotel
    {
        public int HotelId { get; set; }
        public int ServiceId { get; set; }

        public HotelService Service { get; set; }

        public Hotel Hotel { get; set; }
    }
}
