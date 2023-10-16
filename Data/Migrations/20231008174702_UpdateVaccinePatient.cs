using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineProject.Data.Migrations
{
    public partial class UpdateVaccinePatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "VaccinePatients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "VaccinePatients");
        }
    }
}
