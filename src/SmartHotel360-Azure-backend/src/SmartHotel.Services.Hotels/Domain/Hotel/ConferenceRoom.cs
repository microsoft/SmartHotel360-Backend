using System.Collections.Generic;

namespace SmartHotel.Services.Hotels.Domain.Hotel
{
    public class ConferenceRoom
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Rating { get; set; }
        public int PricePerHour { get; set; }
        public int NumPhotos {get; set;}
    }
}
