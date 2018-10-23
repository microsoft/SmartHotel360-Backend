using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Services.BookingProcesor.Exceptions
{

    public class BookingException : Exception
    {

        public BookingExceptionCode Code { get; } = BookingExceptionCode.Unknown;

        public BookingException(string message) : base(message)
        {
        }

        public BookingException(BookingExceptionCode code, string message) : base(message)
        {
            Code = code;
        }
    }
}
