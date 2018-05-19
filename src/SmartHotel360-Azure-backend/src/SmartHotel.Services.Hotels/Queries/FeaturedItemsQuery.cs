using SmartHotel.Services.Hotels.Data;
using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using static System.Linq.Enumerable;
using static System.Linq.Queryable;

namespace SmartHotel.Services.Hotels.Queries
{
    public class FeaturedItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
        public int Price { get; set; }
        public string Picture { get; set; }
    }

    public class FeaturedItemsHotelsQuery
    {
        private readonly HotelsDbContext _db;

        public FeaturedItemsHotelsQuery(HotelsDbContext db)
        {
            _db = db;
        }

        public  Task<IEnumerable<FeaturedItem>> GetForUser(string userid)
        {
            return Get();
        }

        public async Task<IEnumerable<FeaturedItem>> Get()
        {
            var hotels = await _db.Hotels
                .OrderByDescending(hotel => hotel.Visits)
                .Include(hotel => hotel.ConferenceRooms)
                .Include(hotel => hotel.City)
                .Include(hotel => hotel.RoomTypes)
                .Take(2)
                .ToListAsync();

            var featuredHotels = hotels.Select(hotel => new FeaturedItem
            {
                Id = hotel.Id,
                ItemType = "hotel",
                Name = hotel.Name,
                Rating = hotel.Rating,
                City = $"{hotel.City.Name}, {hotel.City.Country}",
                Price = hotel.StarterPricePerNight,
                Picture = $"pichotels/{hotel.Id}_featured.png"
            })
            .ToList();

            var featuredConferenceRooms = hotels.Select(hotel => new FeaturedItem
            {
                Id = hotel.ConferenceRooms.First().Id,
                ItemType = "conferenceRoom",
                Name = hotel.ConferenceRooms.First().Name,
                Rating = hotel.ConferenceRooms.First().Rating,
                City = $"{hotel.City.Name}, {hotel.City.Country}",
                Price = hotel.ConferenceRooms.First().PricePerHour
            })
            .ToList();
            featuredConferenceRooms.ForEach(fi => fi.Picture = $"picconf/{fi.Id}_featured.png");

            return new List<FeaturedItem>
            {
                featuredHotels[0],
                featuredConferenceRooms[0],
                featuredHotels[1],
                featuredConferenceRooms[1]
            };
        }
    }
}
