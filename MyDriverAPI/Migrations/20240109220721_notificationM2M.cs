using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class notificationM2M : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationModel_drivers_DriverId",
                table: "NotificationModel");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationModel_passengers_PassengerId",
                table: "NotificationModel");

            migrationBuilder.DropIndex(
                name: "IX_NotificationModel_DriverId",
                table: "NotificationModel");

            migrationBuilder.DropIndex(
                name: "IX_NotificationModel_PassengerId",
                table: "NotificationModel");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "NotificationModel");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "NotificationModel");

            migrationBuilder.CreateTable(
                name: "DriverNotificationModel",
                columns: table => new
                {
                    DriversId = table.Column<int>(type: "int", nullable: false),
                    NoitficationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverNotificationModel", x => new { x.DriversId, x.NoitficationsId });
                    table.ForeignKey(
                        name: "FK_DriverNotificationModel_NotificationModel_NoitficationsId",
                        column: x => x.NoitficationsId,
                        principalTable: "NotificationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverNotificationModel_drivers_DriversId",
                        column: x => x.DriversId,
                        principalTable: "drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationModelPassenger",
                columns: table => new
                {
                    NoitficationsId = table.Column<int>(type: "int", nullable: false),
                    PassengersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationModelPassenger", x => new { x.NoitficationsId, x.PassengersId });
                    table.ForeignKey(
                        name: "FK_NotificationModelPassenger_NotificationModel_NoitficationsId",
                        column: x => x.NoitficationsId,
                        principalTable: "NotificationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationModelPassenger_passengers_PassengersId",
                        column: x => x.PassengersId,
                        principalTable: "passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverNotificationModel_NoitficationsId",
                table: "DriverNotificationModel",
                column: "NoitficationsId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModelPassenger_PassengersId",
                table: "NotificationModelPassenger",
                column: "PassengersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverNotificationModel");

            migrationBuilder.DropTable(
                name: "NotificationModelPassenger");

            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "NotificationModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "NotificationModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModel_DriverId",
                table: "NotificationModel",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModel_PassengerId",
                table: "NotificationModel",
                column: "PassengerId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationModel_drivers_DriverId",
                table: "NotificationModel",
                column: "DriverId",
                principalTable: "drivers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationModel_passengers_PassengerId",
                table: "NotificationModel",
                column: "PassengerId",
                principalTable: "passengers",
                principalColumn: "Id");
        }
    }
}
