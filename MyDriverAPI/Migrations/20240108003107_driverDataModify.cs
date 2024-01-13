using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class driverDataModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavAreas",
                table: "drivers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavAreas",
                table: "drivers");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "drivers");
        }
    }
}
