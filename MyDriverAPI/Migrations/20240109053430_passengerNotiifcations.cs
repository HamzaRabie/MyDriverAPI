using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class passengerNotiifcations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "NotificationModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "NotificationModel");

        }
    }
}
