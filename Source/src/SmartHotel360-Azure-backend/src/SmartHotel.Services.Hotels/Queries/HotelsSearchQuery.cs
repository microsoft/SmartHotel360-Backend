using SmartHotel.Services.Hotels.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using static System.Linq.Enumerable;
using static System.Linq.Queryable;

namespace SmartHotel.Services.Hotels.Queries
{

    public class HotelSearchFilter
    {
        public int? CityId { get; set; }
        public int? Rating { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }

        public int PageSize { get; set; }
        public int NumPage { get; set; }

        public HotelSearchFilter()
        {
            PageSize = 8;
            NumPage = 0;
        }
    }

    public class HotelSearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Rating { get; set; }
        public int Price { get; set; }
        public int NumPhotos { get; set; }
        public string Picture { get; set; }
    }

    public class HotelsSearchQuery
    {
        private readonly HotelsDbContext _db;

        public HotelsSearchQuery(HotelsDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HotelSearchResult>> Get(HotelSearchFilter filter)
        {
            var query = _db.Hotels
                .OrderByDescending(hotel => hotel.Visits)
                .Include(hotel => hotel.RoomTypes)
                .Include(hotel => hotel.Location)
                .Include(hotel => hotel.City)
                .AsQueryable();

            if (filter.CityId.HasValue)
            {
                query = query.Where(hotel => hotel.City.Id == filter.CityId);
            }

            if (filter.Rating.HasValue)
            {
                query = query.Where(hotel => hotel.Rating >= filter.Rating);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(hotel => hotel.RoomTypes.Min(r => r.Price) >= filter.MinPrice);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(hotel => hotel.RoomTypes.Min(r => r.Price) <= filter.MaxPrice);
            }

            var results = await query.ToListAsync();

            var hotels = results
                .Skip(filter.PageSize * filter.NumPage)
                .Take(filter.PageSize)
                .Select(hotel => new HotelSearchResult
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Rating = hotel.Rating,
                City = $"{hotel.City.Name}, {hotel.City.Country}",
                Price = hotel.StarterPricePerNight,
                NumPhotos = hotel.NumPhotos,
            })
           .ToList();
       
            hotels.ForEach(hsr => hsr.Picture = hsr.NumPhotos > 0 ? $"pichotels/{hsr.Id}_1.png" : "");
            return hotels;
        }
    }
}
