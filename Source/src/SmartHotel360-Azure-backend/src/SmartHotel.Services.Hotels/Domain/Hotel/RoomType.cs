using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Collections.Generic;

namespace SmartHotel.Services.Hotels.Domain
{
    public class RoomType
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }

        public int DoubleBeds { get; set; }
        public int SingleBeds { get; set; }
        public int TwinBeds { get; set; }

        public int NumPhotos { get; set; }

        public IEnumerable<ServicePerRoom> Services { get; set; }
    }
}
