using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Data.Seed
{
    public class CitiesGenerator
    {

        private readonly Dictionary<int, HotelType[]> _hotelTypesPerCity;

        public CitiesGenerator()
        {
            _hotelTypesPerCity = new Dictionary<int, HotelType[]>();


            _hotelTypesPerCity.Add(1, new[] { HotelType.Platinum, HotelType.Gold, HotelType.Spa });
            _hotelTypesPerCity.Add(2, new[] { HotelType.Gold, HotelType.Business, HotelType.Ressort });

            _hotelTypesPerCity.Add(10, new[] { HotelType.Platinum, HotelType.Gold, HotelType.Business });
            _hotelTypesPerCity.Add(11, new[] { HotelType.Family, HotelType.Ressort, HotelType.Economy });
            _hotelTypesPerCity.Add(12, new[] { HotelType.Platinum, HotelType.Gold, HotelType.Family });
            _hotelTypesPerCity.Add(13, new[] { HotelType.Business, HotelType.Ressort, HotelType.Economy });


            _hotelTypesPerCity.Add(20, new[] { HotelType.Gold, HotelType.Spa, HotelType.Business });
            _hotelTypesPerCity.Add(21, new[] { HotelType.Gold, HotelType.Family, HotelType.Economy });
            _hotelTypesPerCity.Add(22, new[] { HotelType.Gold, HotelType.Family, HotelType.Ressort });

            _hotelTypesPerCity.Add(30, new[] { HotelType.Platinum, HotelType.Gold, HotelType.Ressort});
            _hotelTypesPerCity.Add(31, new[] { HotelType.Platinum, HotelType.Gold, HotelType.Ressort });

            _hotelTypesPerCity.Add(40, new[] { HotelType.Platinum, HotelType.Gold, HotelType.Spa });
            _hotelTypesPerCity.Add(41, new[] { HotelType.Gold, HotelType.Business, HotelType.Ressort });
            _hotelTypesPerCity.Add(42, new[] { HotelType.Gold, HotelType.Spa, HotelType.Business });
        }

        public HotelType[] GetHotelTypesPerCity(int cityId) => _hotelTypesPerCity[cityId];


        public List<City> GetCities()
        {
            var cities = new List<City>();

            cities.Add(new City
            {
                Id = 1,
                Name = "Seattle",
                Country = "United States",
                Latitude = 47.608013f,
                Longitude = -122.335167f
            });

            cities.Add(new City
            {
                Id = 2,
                Name = "Seville",
                Country = "Spain",
                Latitude = 37.3827316f,
                Longitude = -5.9833828f
            });

            cities.Add(new City
            {
                Id = 10,
                Name = "New York",
                Country = "United States",
                Latitude = 40.712784f,
                Longitude = -74.005941f
            });

            cities.Add(new City
            {
                Id = 11,
                Name = "Orlando",
                Country = "United States",
                Latitude = 28.553182f,
                Longitude = -81.382293f
            });

            cities.Add(new City
            {
                Id = 12,
                Name = "Newark",
                Country = "United States",
                Latitude = 40.7281349f,
                Longitude = -74.2137461f
            });

            cities.Add(new City
            {
                Id = 13,
                Name = "New Haven",
                Country = "United States",
                Latitude = 41.304892f,
                Longitude = -72.9385377f
            });


            cities.Add(new City
            {
                Id = 20,
                Name = "Barcelona",
                Country = "Spain",
                Latitude = 41.385064f,
                Longitude = 2.173403f
            });

            cities.Add(new City
            {
                Id = 21,
                Name = "Barrie",
                Country = "Canada",
                Latitude = 44.3523058f,
                Longitude = -79.697438f
            });

            cities.Add(new City
            {
                Id = 22,
                Name = "Bari",
                Country = "Italy",
                Latitude = 41.1119782f,
                Longitude = 16.8624782f
            });

            cities.Add(new City
            {
                Id = 30,
                Name = "Rome",
                Country = "Italy",
                Latitude = 41.8983779f,
                Longitude = 12.5265676f
            });

            cities.Add(new City
            {
                Id = 31,
                Name = "Rockford",
                Country = "United States",
                Latitude = 42.2752286f,
                Longitude = -89.0892887f
            });


            cities.Add(new City
            {
                Id = 40,
                Name = "Westminster",
                Country = "United States",
                Latitude = 33.7562592f,
                Longitude = -118.0128342f
            });

            cities.Add(new City
            {
                Id = 41,
                Name = "West Covina",
                Country = "United States",
                Latitude = 34.0561294f,
                Longitude = -117.9220985f
            });

            cities.Add(new City
            {
                Id = 42,
                Name = "Welligton",
                Country = "New Zealand",
                Latitude = -41.286460f,
                Longitude = -174.776236f
            });

            return cities;
        }
    }
}
