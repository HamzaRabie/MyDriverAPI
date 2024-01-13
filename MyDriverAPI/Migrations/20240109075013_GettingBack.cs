using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class GettingBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassengerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassengerId1 = table.Column<int>(type: "int", nullable: true),
                    PassengerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationModel_drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NotificationModel_passengers_PassengerId1",
                        column: x => x.PassengerId1,
                        principalTable: "passengers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModel_DriverId",
                table: "NotificationModel",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModel_PassengerId1",
                table: "NotificationModel",
                column: "PassengerId1");
        }
    }
}
