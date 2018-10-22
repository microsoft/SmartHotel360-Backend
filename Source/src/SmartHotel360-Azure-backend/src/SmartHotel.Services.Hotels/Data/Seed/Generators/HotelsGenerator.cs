using SmartHotel.Services.Hotels.Data.Seed.Generators;
using SmartHotel.Services.Hotels.Domain;
using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;

using static System.Linq.Enumerable;

namespace SmartHotel.Services.Hotels.Data.Seed
{
    public class HotelsGenerator
    {
        public List<Hotel> GetHotels(IEnumerable<City> cities, int hotelsPerCity = 3)
        {
            var pointGenerator = new GeoPointGenerator();
            var hotelNameGenerator = new HotelMetadataGenerator();
            var addressGenerator = new AddressGenerator();
            var cityGenerator = new CitiesGenerator();
            var servicesGenerator = new ServicesGenerator();
            var hotels = new List<Hotel>();

            foreach (var city in cities)
            {
                var hotelTypes = cityGenerator.GetHotelTypesPerCity(city.Id);


                foreach (var hotelType in hotelTypes)
                {
                    var hotelmetadata = hotelNameGenerator.Data.Single(hmd => hmd.HotelType == hotelType);
                    var random = new Random();
                    var rating = hotelmetadata.Rating;
                    var location = pointGenerator.GetClosePoint((city.Latitude, city.Longitude), 1000);
                    var (latitude, longitude) = location;


                    var hotel = new Hotel
                    {
                        Name = $"{hotelmetadata.Prefix} {city.Name}",
                        Description = $"Is a {rating}-Star luxury hotel located in {city.Name} which was opened in {hotelmetadata.Year}. It has quality rooms and a modern, well equipped conference area which can host events of every kind for up to 100 persons. Its an ideal stopover on your journey. Is set in the heart of the lush is perfect for holiday or business. If you just wish to relax there are many walks, cycle rides or car excursions to local quaint and historical villages and towns in picturesque surroundings. If you don't feel like outdoor exercise you can relax either in our Finnish sauna, bio sauna, infrared sauna, jacuzzi. Or, if your feeling up to it, why not work out gently in the gym. After a relaxing dinner and a quiet drink in our Bistro bar sleep should be no problem in one of our cosy and comfortable rooms. Our young and friendly staff are here to welcome and serve you. Hope to see you soon!",
                        City = city,
                        Address = new Address
                        {
                            PostCode = addressGenerator.GetPostCode(),
                            Street = addressGenerator.GetStreet()
                        },
                        CheckinTime = new TimeSpan(15, 0, 0),
                        CheckoutTime = new TimeSpan(12, 0, 0),
                        Location = new Location { Latitude = latitude, Longitude = longitude },
                        Rating = rating,
                        Visits = random.Next(10000, 30000),
                        NumPhotos = 1,
                        RoomTypes = new RoomType[] {
                            GetRoomType(hotelmetadata.RoomTypes.single),
                            GetRoomType(hotelmetadata.RoomTypes.@double)
                        },
                        ConferenceRooms = new[]
                        {
                            new ConferenceRoom
                            {
                                Name = $"{HotelMetadataGenerator.GetConferenceRoomName(hotelType)} room",
                                Rating = rating,
                                Capacity = 40,
                                PricePerHour = 190,
                                NumPhotos = 3
                            }
                        }
                    };
                    hotel.Services = servicesGenerator.GetHotelServicesByHotelType(hotelType)
                        .Select(hs => new ServicePerHotel()
                        {
                            Service = hs,
                            Hotel = hotel
                        }).ToList();
                    hotels.Add(hotel);
                }
            }

            return hotels;
        }


        private RoomType GetRoomType(int type)
        {
            var serviceGenerator = new ServicesGenerator();
            RoomType room = null;
            switch (type)
            {
                case HotelSeedMetada.ROOMTYPE_SINGLE:
                    room = new RoomType
                    {
                        Name = "Single room",
                        Capacity = 1,
                        Description = "Our single rooms have an area of 26m² with a 1.60m Queen size bed.",
                        Price = 180,
                        NumPhotos = 3,
                        SingleBeds = 1
                    };
                    room.Services = serviceGenerator.GetRoomServices()
                        .Select(rs => new ServicePerRoom() { RoomType = room, Service = rs }).ToList();
                    return room;

                case HotelSeedMetada.ROOMTYPE_DOUBLE:
                    room = new RoomType
                    {
                        Name = "Double room",
                        Capacity = 2,
                        Description = "The doubles are 32m² with French Size twin beds of 1.40m",
                        Price = 300,
                        NumPhotos = 4,
                        TwinBeds = 2
                    };
                    room.Services = serviceGenerator.GetRoomServices()
                        .Select(rs => new ServicePerRoom() { RoomType = room, Service = rs }).ToList();
                    return room;

                case HotelSeedMetada.ROOMTYPE_DOUBLE2:
                    room = new RoomType
                    {
                        Name = "Double room",
                        Capacity = 2,
                        Description = "The doubles are 40m² with a 3.20m double Queen size bed",
                        Price = 400,
                        NumPhotos = 2,
                        TwinBeds = 2,
                    };
                    room.Services = serviceGenerator.GetRoomServices()
                        .Select(rs => new ServicePerRoom() { RoomType = room, Service = rs }).ToList();
                    return room;

                case HotelSeedMetada.ROOMTYPE_LUXURY:
                    room = new RoomType
                    {
                        Name = "Luxury room",
                        Capacity = 2,
                        Description = "Luxury room is 42m² with a 3.20m double Queen size bed.",
                        Price = 500,
                        NumPhotos = 3,
                        DoubleBeds = 1,
                    };
                    room.Services = serviceGenerator.GetRoomServices()
                        .Select(rs => new ServicePerRoom() { RoomType = room, Service = rs }).ToList();
                    return room;
                default:
                    return null;
            }
        }
    }
}
