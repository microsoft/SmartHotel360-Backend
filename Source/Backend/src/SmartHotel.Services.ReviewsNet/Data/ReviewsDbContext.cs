using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHotel.Services.ReviewsNet.Data
{
    public class ReviewsDbContext : DbContext
    {

        public DbSet<Review> Reviews { get; set; }

        public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().ToTable("review");
            modelBuilder.Entity<Review>().Property(r => r.Id).ForNpgsqlUseSequenceHiLo("hibernate_sequence", "public");
            modelBuilder.Entity<Review>().Property(r => r.Id).HasColumnName("id");
            modelBuilder.Entity<Review>().Property(r => r.HotelId).HasColumnName("hotel_id");
            modelBuilder.Entity<Review>().Property(r => r.UserId).HasColumnName("user_id");
            modelBuilder.Entity<Review>().Property(r => r.UserName).HasColumnName("user_name");
            modelBuilder.Entity<Review>().Property(r => r.Description).HasColumnName("description");
            modelBuilder.Entity<Review>().Property(r => r.Submitted).HasColumnName("submitted");
            modelBuilder.Entity<Review>().Ignore(r => r.FormattedDate);
            modelBuilder.Entity<Review>().Property(r => r.Description).HasMaxLength(1024);

        }
    }
}
