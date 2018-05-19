using SmartHotel.Services.Bookings.Data.Repositories;
using SmartHotel.Services.Bookings.Domain;
using SmartHotel.Services.Bookings.Domain.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Bookings.Commands
{
    public class BookingRequest
    {
        public int HotelId { get; set; }
        public string UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public byte Adults { get; set; }
        public byte Kids { get; set; }
        public byte Babies { get; set; }
        public int RoomType { get; set; }
        public int Price { get; set; }
    }


    public class CreateBookingCommand
    {
        private readonly BookingRepository _bookingRepository;
        private readonly UnitOfWork _uow;

        public CreateBookingCommand(
            BookingRepository bookingRepostiory,
            UnitOfWork uow
            )
        {
            _bookingRepository = bookingRepostiory;
            _uow = uow;
        }

        public async Task<bool> Execute(BookingRequest bookingRequest)
        {
            var booking = new Booking
            {
                IdHotel = bookingRequest.HotelId,
                ClientEmail = bookingRequest.UserId,
                CheckInDate = bookingRequest.From,
                CheckOutDate = bookingRequest.To,
                TotalCost = bookingRequest.Price,
                NumberOfAdults = bookingRequest.Adults,
                NumberOfChildren = bookingRequest.Kids,
                NumberOfBabies = bookingRequest.Babies,
                IdRoomType = bookingRequest.RoomType 
            };

            _bookingRepository.Add(booking);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
