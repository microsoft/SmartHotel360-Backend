using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Data.Seed
{
    public class HotelSeedMetada
    {

        public const int ROOMTYPE_SINGLE = 1;
        public const int ROOMTYPE_DOUBLE = 2;
        public const int ROOMTYPE_DOUBLE2 = 3;
        public const int ROOMTYPE_LUXURY = 4;


        public string Prefix { get; }
        public int Rating { get; }

        public HotelType HotelType { get; }

        public int Year { get; }

        public HotelSeedMetada(string name, int rating, HotelType type, int year)
        {
            HotelType = type;
            Prefix = name;
            Rating = rating;
            Year = year;
        }

        public (int single, int @double) RoomTypes
        {
            get
            {
                switch (HotelType)
                {
                    case HotelType.Platinum:
                    case HotelType.Gold:
                        return (ROOMTYPE_SINGLE, ROOMTYPE_LUXURY);
                    case HotelType.Family:
                    case HotelType.Business:
                    case HotelType.Economy:
                        return (ROOMTYPE_SINGLE, ROOMTYPE_DOUBLE);
                    case HotelType.Spa:
                    case HotelType.Ressort:
                        return (ROOMTYPE_SINGLE, ROOMTYPE_DOUBLE2);
                    default:
                        return (ROOMTYPE_SINGLE, ROOMTYPE_DOUBLE);
                }

            }
        }
    }


    public enum HotelType
    {
        Platinum = 1,
        Gold = 2,
        Spa = 3,
        Business= 4,
        Family= 5,
        Ressort = 6,
        Economy = 7
    }


    public class HotelMetadataGenerator
    {


        private static List<string> ConferenceRoomNames = new List<string>
        {
            "NetCore",
            "Azure",
            "CosmosDb",
            "VisualStudio",
            "Xamarin",
            "Connect()"
        };

        public static string GetConferenceRoomName(HotelType type)
        {
            return ConferenceRoomNames[(int)type % ConferenceRoomNames.Count];
        }


        private static List<HotelSeedMetada> _data = new List<HotelSeedMetada>
            {
                new HotelSeedMetada("Sh360 Platinum", 5,HotelType.Platinum, 1996),
                new HotelSeedMetada("Sh360 Gold", 5,HotelType.Gold, 1998),
                new HotelSeedMetada("Sh360 Spa", 5,HotelType.Spa, 2000),
                new HotelSeedMetada("Sh360 Business", 4,HotelType.Business, 2006),
                new HotelSeedMetada("Sh360 Family", 4,HotelType.Family, 2010),
                new HotelSeedMetada("Sh360 Ressort", 4,HotelType.Ressort, 2008),
                new HotelSeedMetada("Sh360 Economy", 3,HotelType.Economy, 2015)
            };
        public IEnumerable<HotelSeedMetada> Data => _data;
    }

}
