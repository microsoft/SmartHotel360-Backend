using System;
using System.Collections.Generic;


using static System.Linq.Enumerable;

namespace SmartHotel.Services.Hotels.Domain.Hotel
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public Location Location { get; set; }
        public City City {get;set;}
        public int Rating { get; set; }
        public int StarterPricePerNight => RoomTypes.Min(room => room.Price);
        public IEnumerable<RoomType> RoomTypes { get; set; }
        public IEnumerable<ConferenceRoom> ConferenceRooms { get; set; }

        public IEnumerable<ServicePerHotel> Services { get; set; }
        public int Visits { get; set; }
        public TimeSpan CheckinTime { get; set; }
        public TimeSpan CheckoutTime { get; set; }

        public int NumPhotos { get; set; }

    }
}
