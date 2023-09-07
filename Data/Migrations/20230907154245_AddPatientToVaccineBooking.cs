using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineWebApp.Data.Migrations
{
    public partial class AddPatientToVaccineBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_vaccineBookings_VaccineID",
                table: "vaccineBookings",
                column: "VaccineID");

            migrationBuilder.AddForeignKey(
                name: "FK_vaccineBookings_vaccine_VaccineID",
                table: "vaccineBookings",
                column: "VaccineID",
                principalTable: "vaccine",
                principalColumn: "VaccineID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vaccineBookings_vaccine_VaccineID",
                table: "vaccineBookings");

            migrationBuilder.DropIndex(
                name: "IX_vaccineBookings_VaccineID",
                table: "vaccineBookings");
        }
    }
}
