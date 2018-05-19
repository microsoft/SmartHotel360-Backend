using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Services.BookingProcesor.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DefaultPicture { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
    }

    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string DefaultPicture { get; set; }
        public int Price { get; set; }
        public IEnumerable<string> Pictures { get; set; }
    }
}
