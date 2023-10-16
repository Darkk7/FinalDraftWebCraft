using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineProject.Data.Migrations
{
    public partial class UpdateVaccinePatients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "VaccinePatients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "VaccinePatients");
        }
    }
}
