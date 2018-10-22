using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Services.BookingProcesor.Exceptions
{
    public enum BookingExceptionCode
    {
        Unknown,
        HotelNotFound,
        GuestsRoomsMissmatch,
        InvalidPrice
    }
}
