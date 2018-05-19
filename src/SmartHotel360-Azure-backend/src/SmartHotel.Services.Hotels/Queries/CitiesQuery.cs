using SmartHotel.Services.Hotels.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using static System.Linq.Queryable;
using System;

namespace SmartHotel.Services.Hotels.Queries
{
    public class CityResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class CitiesQuery
    {
        private readonly HotelsDbContext _db;

        public CitiesQuery(HotelsDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CityResult>> Get(string name = "", int take = 5)
        {
            return await _db
                .Cities
                .Where(city => city.Name.StartsWith(name))
                .Take(take)
                .Select(city => new CityResult
                {
                    Id = city.Id,
                    Name = city.Name,
                    Country = city.Country
                })
                .ToListAsync();
        }

        public Task<IEnumerable<CityResult>> GetDefaultCities()
        {
            return Task.FromResult(new[]
            {
                new CityResult() { Id = 10, Name = "New York", Country = "United States"},
                new CityResult() { Id = 11, Name = "New Orleans", Country = "United States"},
                new CityResult() { Id = 30,  Name = "Barcelona", Country = "Spain"},
                new CityResult() { Id = 20,  Name = "Rome", Country = "Italy" }
            } as IEnumerable<CityResult>);
        }
    }
}
