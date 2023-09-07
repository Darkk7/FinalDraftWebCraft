using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineWebApp.Data.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VaccinationBookingID",
                table: "vaccineBookings",
                newName: "VaccineBookingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VaccineBookingID",
                table: "vaccineBookings",
                newName: "VaccinationBookingID");
        }
    }
}
