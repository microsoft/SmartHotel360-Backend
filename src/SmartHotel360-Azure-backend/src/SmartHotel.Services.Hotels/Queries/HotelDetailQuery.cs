using SmartHotel.Services.Hotels.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using static System.Linq.Enumerable;
using static System.Linq.Queryable;
using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Linq;

namespace SmartHotel.Services.Hotels.Queries
{
    public class HotelDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int PricePerNight { get; set; }
        public string DefaultPicture { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public RoomSummary[] Rooms { get; set; }
        public IEnumerable<HotelService> Services { get; set; }
        public int NumPhotos { get; set; }
    }

    public class RoomSummary
    {
        public int RoomId { get; set; }
        public string RommName { get; set; }
        public decimal RoomPrice { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal OriginalRoomPrice { get; set; }
        public decimal LocalRoomPrice { get; set; }
        public decimal LocalOriginalRoomPrice { get; set; }

        public string BadgeSymbol { get; set; }
    }

    public class RoomDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string DefaultPicture { get; set; }
        public IEnumerable<string> Pictures { get; set; }
        public IEnumerable<RoomService> Services { get; set; }
        public int TwinBeds { get; set; }
        public int DoubleBeds { get; set; }
        public int SingleBeds { get; set; }
        public int NumPhotos { get; set; }
    }



    public class HotelDetailQuery
    {
        private readonly HotelsDbContext _db;

        public HotelDetailQuery(HotelsDbContext db)
        {
            _db = db;
        }

        public async Task<HotelDetail> Get(int hotelId, double discount)
        {
            var hotel = await _db.Hotels
                .Include(h => h.RoomTypes)
                .Include(h => h.Services).ThenInclude(sph => sph.Service)
                .Include(h=>h.City)
                .SingleOrDefaultAsync(h => h.Id == hotelId);

            var detail = new HotelDetail()
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                Rating = hotel.Rating,
                City = $"{hotel.City.Name}, {hotel.City.Country}",
                Street = hotel.Address.Street,
                Latitude = hotel.Location.Latitude,
                Longitude = hotel.Location.Longitude,
                PricePerNight = hotel.StarterPricePerNight,
                CheckInTime = hotel.CheckinTime,
                CheckOutTime = hotel.CheckoutTime,
                NumPhotos = hotel.NumPhotos,
                DefaultPicture = $"pichotels/{hotel.Id}_default.png",
                Pictures = Enumerable.Range(1, hotel.NumPhotos)
                    .Select(idx => $"pichotels/{hotel.Id}_{idx}.png"),
                Services = hotel.Services.Select (sph => sph.Service),
                Rooms = hotel.RoomTypes.Select(roomType => new RoomSummary()
                {
                    RommName = roomType.Name,
                    RoomId = roomType.Id,
                    RoomPrice = roomType.Price - (roomType.Price*(decimal)discount),
                    DiscountApplied = (decimal)discount,
                    OriginalRoomPrice = roomType.Price
                }).ToArray()
            };

            return detail;
        }

        public async Task<IEnumerable<RoomDetail>> GetRoomsByHotel(int hotelId)
        {
            var hotel = await _db.Hotels
                .Include(h => h.RoomTypes).ThenInclude(r => r.Services).ThenInclude(rs => rs.Service)
                .SingleAsync(h => h.Id == hotelId);

            if (hotel == null)
            {
                return null;
            }

            var rooms = hotel.RoomTypes.Select(roomType => new RoomDetail
            {

                Id = roomType.Id,
                Name = roomType.Name,
                Description = roomType.Description,
                Capacity = roomType.Capacity,
                DefaultPicture = $"picrooms/{roomType.Id}_default.png",
                Services = roomType.Services.Select(s => s.Service),
                DoubleBeds = roomType.DoubleBeds,
                TwinBeds = roomType.TwinBeds,
                SingleBeds = roomType.SingleBeds,
                NumPhotos  = roomType.NumPhotos
            }).ToList();

            rooms.ForEach(rd => rd.Pictures =
                Enumerable.Range(1, rd.NumPhotos)
                .Select(idx => $"picrooms/{rd.Id}_{idx}.png"));
            return rooms;
        }
    }
}

