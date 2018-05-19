using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Data.Seed.Generators
{
    public class ServicesGenerator
    {

        private readonly List<HotelService> _commonServices;
        private readonly List<HotelService> _luxuryServices;
        private readonly List<HotelService> _spaServices;
        private readonly List<HotelService> _businessServices;
        private readonly List<HotelService> _familyServices;

        public ServicesGenerator()
        {
            _commonServices = new List<HotelService>();
            _commonServices.Add(new HotelService() { Id = 1, Name = "Free Wi-fi" });
            _commonServices.Add(new HotelService() { Id = 2, Name = "Parking" });
            _commonServices.Add(new HotelService() { Id = 3, Name = "TV" });
            _commonServices.Add(new HotelService() { Id = 4, Name = "Air conditioning" });
            _commonServices.Add(new HotelService() { Id = 5, Name = "Dryer" });
            _commonServices.Add(new HotelService() { Id = 6, Name = "Indoor fireplace" });
            _commonServices.Add(new HotelService() { Id = 8, Name = "Breakfast" });
            _commonServices.Add(new HotelService() { Id = 10, Name = "Airport shuttle" });
            _commonServices.Add(new HotelService() { Id = 13, Name = "Gym" });
            _commonServices.Add(new HotelService() { Id = 15, Name = "Restaurant" });
            _commonServices.Add(new HotelService() { Id = 16, Name = "Wheelchair accessible" });
            _commonServices.Add(new HotelService() { Id = 17, Name = "Elevator" });

            _luxuryServices = new List<HotelService>();
            _luxuryServices.Add(new HotelService() { Id = 12, Name = "Fitness centre" });

            _spaServices = new List<HotelService>();
            _spaServices.Add(new HotelService() { Id = 11, Name = "Swimming pool" });
            _spaServices.Add(new HotelService() { Id = 14, Name = "Hot tub" });


            _businessServices = new List<HotelService>();
            _businessServices.Add(new HotelService() { Id = 7, Name = "Laptop workspace" });

            _familyServices = new List<HotelService>();
            _businessServices.Add(new HotelService() { Id = 9, Name = "Kid friendly" });
        }

        public IEnumerable<HotelService> GetAllHotelServices()
        {
            return _commonServices.Concat(_businessServices).Concat(_familyServices).Concat(_spaServices).Concat(_luxuryServices);
        }

        public IEnumerable<HotelService> GetHotelServicesByHotelType(HotelType  type)
        {
            switch (type)
            {
                case HotelType.Platinum:
                case HotelType.Gold:
                    return GetAllHotelServices();
                case HotelType.Spa:
                    return _commonServices.Concat(_spaServices);
                case HotelType.Business:
                    return _commonServices.Concat(_businessServices);
                case HotelType.Family:
                    return _commonServices.Concat(_familyServices);
                case HotelType.Ressort:
                    return _commonServices.Concat(_spaServices).Concat(_luxuryServices);
                case HotelType.Economy:
                    return _commonServices;
                default:
                    return _commonServices;
            }
        }

        public IEnumerable<RoomService> GetRoomServices()
        {
            return new[]
            {
                new RoomService() { Id = 1, Name = "Bath"},
                new RoomService() { Id = 2, Name = "Shared bath"},
                new RoomService() { Id = 3, Name = "Air conditioning"},
                new RoomService() { Id = 4, Name = "Green"},
                new RoomService() { Id = 5, Name = "Work Table"}
            };
        }
    }
}
