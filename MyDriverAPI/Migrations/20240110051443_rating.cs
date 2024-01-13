using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "trips");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "drivers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "drivers");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "trips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
