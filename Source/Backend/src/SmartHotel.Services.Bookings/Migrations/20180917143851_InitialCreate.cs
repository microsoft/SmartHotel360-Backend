using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartHotel.Services.Bookings.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckInDate = table.Column<DateTime>(type: "date", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "date", nullable: false),
                    ClientEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdHotel = table.Column<int>(type: "int", nullable: false),
                    IdRoomType = table.Column<int>(type: "int", nullable: false),
                    NumberOfAdults = table.Column<byte>(type: "tinyint", nullable: false),
                    NumberOfBabies = table.Column<byte>(type: "tinyint", nullable: false),
                    NumberOfChildren = table.Column<byte>(type: "tinyint", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(7,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");
        }
    }
}
