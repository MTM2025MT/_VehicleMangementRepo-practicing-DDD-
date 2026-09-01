using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle_Management.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _fuelpolicy = table.Column<int>(type: "int", nullable: false),
                    TripStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TripEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    timeframe = table.Column<TimeSpan>(type: "time", nullable: false),
                    Employee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vehicle_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    License_Plate = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _classification = table.Column<int>(type: "int", nullable: false),
                    _status = table.Column<int>(type: "int", nullable: false),
                    Odometer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.License_Plate);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
