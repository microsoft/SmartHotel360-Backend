using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Domain.Hotel
{
    public class ServicePerRoom
    {
        public int RoomTypeId { get; set; }
        public int ServiceId { get; set; }

        public RoomService Service { get; set; }

        public RoomType RoomType { get; set; }
    }
}
