using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotels.Data;

using static System.Linq.Enumerable;
using static System.Linq.Queryable;

namespace SmartHotel.Services.Hotels.Queries
{

    public class ConferenceRoomSearchFilter
    {
        public int? CityId { get; set; }
        public int? Rating { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? Guests { get; set; }
    }

    public class ConferenceRoomSearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
        public int PricePerHour { get; set; }
        public int Capacity { get; set; }
        public string Picture { get; set; }
        public int NumPhotos { get; set; }
    }

    public class ConferenceRoomSearchQuery
    {
        private readonly HotelsDbContext _db;

        public ConferenceRoomSearchQuery(HotelsDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ConferenceRoomSearchResult>> Get(ConferenceRoomSearchFilter filter)
        {
            var query = _db.Hotels
                .OrderByDescending(hotel => hotel.Visits)
                .Include(hotel => hotel.ConferenceRooms)
                .Include(hotel => hotel.Location)
                .Include(hotel => hotel.City)
                .Include(hotel => hotel.Address)
                .AsQueryable();

            if (filter.CityId.HasValue)
            {
                query = query.Where(hotel => hotel.City.Id == filter.CityId);
            }

            var conferencesQuery = query.SelectMany(hotel => hotel.ConferenceRooms.Select(conferenceRoom => new ConferenceRoomSearchResult
            {
                Id = conferenceRoom.Id,
                Name = conferenceRoom.Name,
                HotelId = hotel.Id,
                HotelName = hotel.Name,
                Rating = conferenceRoom.Rating,
                City = $"{hotel.City.Name}, {hotel.City.Country}",
                PricePerHour = conferenceRoom.PricePerHour,
                Capacity = conferenceRoom.Capacity,
                NumPhotos = conferenceRoom.NumPhotos
            }));

            if (filter.Rating.HasValue)
            {
                query = query.Where(conferenceRoom => conferenceRoom.Rating >= filter.Rating);
            }

            if (filter.MinPrice.HasValue)
            {
                conferencesQuery = conferencesQuery.Where(conferenceRoom => conferenceRoom.PricePerHour >= filter.MinPrice);
            }

            if (filter.MaxPrice.HasValue)
            {
                conferencesQuery = conferencesQuery.Where(conferenceRoom => conferenceRoom.PricePerHour <= filter.MaxPrice);
            }

            if (filter.Guests.HasValue)
            {
                conferencesQuery = conferencesQuery.Where(conferenceRoom => filter.Guests.Value <= conferenceRoom.Capacity);
            }
            var result = await conferencesQuery.ToListAsync();

            result.ForEach(crs => crs.Picture = crs.NumPhotos > 0 ? $"picconf/{crs.Id}_1.png" : "");

            return result;
        }
    }
}
