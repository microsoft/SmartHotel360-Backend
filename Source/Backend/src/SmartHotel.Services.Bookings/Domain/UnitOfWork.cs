using SmartHotel.Services.Bookings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Bookings.Domain
{
    public class UnitOfWork
    {
        private readonly BookingsDbContext _db;

        public UnitOfWork(BookingsDbContext db)
        {
            _db = db;
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
