using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Hotels.Data;
using SmartHotel.Services.Hotels.Domain.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Hotels.Queries
{
    public class ServicesQuery
    {
        private readonly HotelsDbContext _db;
        public ServicesQuery(HotelsDbContext db) => _db = db;

        public async Task<IEnumerable<HotelService>> GetAllHotelServices()
        {
            return await _db.HotelServices.OrderBy(s => s.Id).ToListAsync();
        }

        public async Task<IEnumerable<RoomService>> GetAllRoomServices()
        {
            return await _db.RoomServices.OrderBy(s => s.Id).ToListAsync();
        }
    }
}
