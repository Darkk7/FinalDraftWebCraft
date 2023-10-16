using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaccineProject.Data.Migrations
{
    public partial class UpdateVaccineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeID",
                table: "vaccine",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_vaccine_TypeID",
                table: "vaccine",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_vaccine_vaccineType_TypeID",
                table: "vaccine",
                column: "TypeID",
                principalTable: "vaccineType",
                principalColumn: "TypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vaccine_vaccineType_TypeID",
                table: "vaccine");

            migrationBuilder.DropIndex(
                name: "IX_vaccine_TypeID",
                table: "vaccine");

            migrationBuilder.DropColumn(
                name: "TypeID",
                table: "vaccine");
        }
    }
}
