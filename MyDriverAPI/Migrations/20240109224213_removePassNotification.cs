using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class removePassNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverNotificationModel_NotificationModel_NoitficationsId",
                table: "DriverNotificationModel");

            migrationBuilder.DropTable(
                name: "NotificationModelPassenger");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationModel",
                table: "NotificationModel");

            migrationBuilder.RenameTable(
                name: "NotificationModel",
                newName: "notifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notifications",
                table: "notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverNotificationModel_notifications_NoitficationsId",
                table: "DriverNotificationModel",
                column: "NoitficationsId",
                principalTable: "notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverNotificationModel_notifications_NoitficationsId",
                table: "DriverNotificationModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notifications",
                table: "notifications");

            migrationBuilder.RenameTable(
                name: "notifications",
                newName: "NotificationModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationModel",
                table: "NotificationModel",
                column: "Id");

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
                name: "IX_NotificationModelPassenger_PassengersId",
                table: "NotificationModelPassenger",
                column: "PassengersId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverNotificationModel_NotificationModel_NoitficationsId",
                table: "DriverNotificationModel",
                column: "NoitficationsId",
                principalTable: "NotificationModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
