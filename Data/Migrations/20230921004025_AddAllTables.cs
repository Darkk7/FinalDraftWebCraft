using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineProject.Data.Migrations
{
    public partial class AddAllTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeLogins",
                columns: table => new
                {
                    EmpID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLogins", x => x.EmpID);
                });

            migrationBuilder.CreateTable(
                name: "vaccine",
                columns: table => new
                {
                    VaccineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VaccineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosesNeeded = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LowQuantity = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedAidNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vaccine", x => x.VaccineID);
                });

            migrationBuilder.CreateTable(
                name: "VaccinePatients",
                columns: table => new
                {
                    VaccinePatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateAddedToSystem = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinePatients", x => x.VaccinePatientID);
                });

            migrationBuilder.CreateTable(
                name: "vaccineType",
                columns: table => new
                {
                    TypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vaccineType", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "vaccineBookings",
                columns: table => new
                {
                    VaccineBookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VaccineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoseNumber = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    VaccineID = table.Column<int>(type: "int", nullable: true),
                    VaccinePatientID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vaccineBookings", x => x.VaccineBookingID);
                    table.ForeignKey(
                        name: "FK_vaccineBookings_vaccine_VaccineID",
                        column: x => x.VaccineID,
                        principalTable: "vaccine",
                        principalColumn: "VaccineID");
                    table.ForeignKey(
                        name: "FK_vaccineBookings_VaccinePatients_VaccinePatientID",
                        column: x => x.VaccinePatientID,
                        principalTable: "VaccinePatients",
                        principalColumn: "VaccinePatientID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_vaccineBookings_VaccineID",
                table: "vaccineBookings",
                column: "VaccineID");

            migrationBuilder.CreateIndex(
                name: "IX_vaccineBookings_VaccinePatientID",
                table: "vaccineBookings",
                column: "VaccinePatientID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeLogins");

            migrationBuilder.DropTable(
                name: "vaccineBookings");

            migrationBuilder.DropTable(
                name: "vaccineType");

            migrationBuilder.DropTable(
                name: "vaccine");

            migrationBuilder.DropTable(
                name: "VaccinePatients");
        }
    }
}
