using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class tripMany2Many : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "trips");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "trips");

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "NotificationModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "NotificationModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DriverTrip",
                columns: table => new
                {
                    DriversId = table.Column<int>(type: "int", nullable: false),
                    TripsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverTrip", x => new { x.DriversId, x.TripsId });
                    table.ForeignKey(
                        name: "FK_DriverTrip_drivers_DriversId",
                        column: x => x.DriversId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverTrip_trips_TripsId",
                        column: x => x.TripsId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassengerTrip",
                columns: table => new
                {
                    PassengersId = table.Column<int>(type: "int", nullable: false),
                    TripsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerTrip", x => new { x.PassengersId, x.TripsId });
                    table.ForeignKey(
                        name: "FK_PassengerTrip_passengers_PassengersId",
                        column: x => x.PassengersId,
                        principalTable: "passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PassengerTrip_trips_TripsId",
                        column: x => x.TripsId,
                        principalTable: "trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverTrip_TripsId",
                table: "DriverTrip",
                column: "TripsId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerTrip_TripsId",
                table: "PassengerTrip",
                column: "TripsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverTrip");

            migrationBuilder.DropTable(
                name: "PassengerTrip");

            migrationBuilder.DropColumn(
                name: "Destination",
                table: "NotificationModel");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "NotificationModel");

            migrationBuilder.AddColumn<string>(
                name: "DriverId",
                table: "trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassengerId",
                table: "trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
