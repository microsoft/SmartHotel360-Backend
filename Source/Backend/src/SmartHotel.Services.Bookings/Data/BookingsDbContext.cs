using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Bookings.Domain.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.Bookings.Data
{
    public class BookingsDbContext : DbContext
    {
        public BookingsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().ToTable("Booking");

            modelBuilder.Entity<Booking>()
                .Property(b => b.ClientEmail).IsRequired().
                HasMaxLength(50);

            modelBuilder.Entity<Booking>().Property(e => e.TotalCost)
                .HasColumnType("decimal(7,2)");

            modelBuilder.Entity<Booking>()
                .Property(b => b.CheckInDate).HasColumnType("date");

            modelBuilder.Entity<Booking>()
                .Property(b => b.CheckOutDate).HasColumnType("date");

        }

        public DbSet<Booking> Bookings { get; set; }
    }
}
