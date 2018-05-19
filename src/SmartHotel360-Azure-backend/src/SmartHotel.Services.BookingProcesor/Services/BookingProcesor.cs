using SmartHotel.Services.BookingProcesor.Exceptions;
using SmartHotel.Services.BookingProcesor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotel.Services.BookingProcesor.Services
{
    public class BookingProcesor
    {
        private readonly HotelService _hotelService;
        private readonly BookingService _bookingService;

        public BookingProcesor()
        {
            _hotelService = new HotelService();
            _bookingService = new BookingService();
        }

        public async Task<bool> ProcessAsync(BookingRequest request)
        {
            var hotel = await _hotelService.GetHotelAsync(request.HotelId);

            if (hotel == null)
            {
                new BookingException(BookingExceptionCode.HotelNotFound, $"Hotel with {request.HotelId} not found");
            }

            int totalCapacity = hotel.Rooms.Sum(room => room.Capacity);
            int totalGuests = request.Adults + request.Kids + request.Babies;

            if (totalCapacity < totalGuests)
            {
                throw new BookingException(BookingExceptionCode.GuestsRoomsMissmatch, "Number of guests doesn't match selected rooms capacity");
            }

            int totalPrice = 0;

            for (int i = 0; i < request.Rooms.Count(); i++)
            {
                var roomRequest = request.Rooms.ElementAt(i);
                CalculatePrice(out var price, hotel, roomRequest);
                totalPrice += price;
            }
                        
            if (request.Price != totalPrice)
            {
                throw new BookingException(BookingExceptionCode.InvalidPrice, "Invalid price");
            }

            return await _bookingService.RequestBookingAsync(request);
        }

        private void CalculatePrice(out int price, Hotel hotel, RoomBookingRequest roomRequest)
        {
            var roomType = hotel.Rooms.First(r => r.Id == roomRequest.RoomType);
            price = roomType.Price * roomRequest.Quantity;
        }
    }
}
