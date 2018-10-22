using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using SmartHotel.Services.BookingProcesor.Models;

namespace SmartHotel.Services.BookingProcesor.Services
{
    public class HotelService
    {
        private readonly string _hotelsApiUrl;
        public HotelService()
        {
            _hotelsApiUrl = ConfigurationManager.AppSettings["HotelsApiUrl"];

            if (string.IsNullOrEmpty(_hotelsApiUrl))
            {
                throw new ArgumentNullException("HotelsApiUrl setting missing");
            }
        }

        public async Task<Hotel> GetHotelAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{_hotelsApiUrl}/api/hotels/{id}";
                var response = await httpClient.GetAsync(url);
                var hotel = await response.Content.ReadAsAsync<Hotel>();
                return hotel;
            }                
        }
    }
}
