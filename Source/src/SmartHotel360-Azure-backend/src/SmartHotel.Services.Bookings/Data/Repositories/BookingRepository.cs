using SmartHotel.Services.Bookings.Domain.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Bookings.Data.Repositories
{
    public class BookingRepository
    {
        private readonly BookingsDbContext _db;

        public BookingRepository(BookingsDbContext db)
        {
            _db = db;
        }

        public void Add(Booking booking)
        {
            _db.Bookings.Add(booking);
        }
    }
}
