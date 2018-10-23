using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartHotel.Services.Hotels.Data.Seed.Generators;
using SmartHotel.Services.Hotels.Domain;
using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Data.Seed
{
    public class HotelsDbContextSeed
    {
        public static void Seed(HotelsDbContext db)
        {
            bool alreadySeeded = db.Cities.Any();

            if (alreadySeeded)
            {
                return;
            }

            var servicesGenerator = new ServicesGenerator();
            var citiesGenerator = new CitiesGenerator();
            var hotelsGenerator = new HotelsGenerator();
            var reviewGenerator = new ReviewGenerator();

            // Seed services
            var hotelServices = servicesGenerator.GetAllHotelServices();
            foreach (var service in hotelServices)
            {
                db.HotelServices.Add(service);
            }
            db.SaveChanges();

            // Seed services
            var roomServices = servicesGenerator.GetRoomServices();
            foreach (var service in roomServices)
            {
                db.RoomServices.Add(service);
            }
            db.SaveChanges();

            // Seed citites
            var cities = citiesGenerator.GetCities();
            cities.ForEach(city => db.Cities.Add(city));
            db.SaveChanges();

            var hotels = hotelsGenerator.GetHotels(cities);
            hotels.ForEach(hotel => db.Hotels.Add(hotel));
            db.SaveChanges();

            var reviews = reviewGenerator.GetReviews(hotels);
            reviews.ForEach(review => db.Reviews.Add(review));
            db.SaveChanges();
        }
    }
}
