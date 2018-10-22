using SmartHotel.Services.BookingProcesor.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Services.BookingProcesor.Services
{
    public class BookingService
    {
        private  readonly string _bookingApiUrl;

        public BookingService()
        {
            _bookingApiUrl = ConfigurationManager.AppSettings["BookingsApiUrl"];

            if (string.IsNullOrEmpty(_bookingApiUrl))
            {
                throw new ArgumentNullException("BookingsApiUrl setting missing");
            }
        }

        public async Task<bool> RequestBookingAsync(BookingRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{_bookingApiUrl}/api/bookings";

                var response = await httpClient.PostAsJsonAsync(url, request);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
