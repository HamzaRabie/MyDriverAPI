using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDriver.Migrations
{
    /// <inheritdoc />
    public partial class ss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.AddColumn<int>(
                name: "PassengerId1",
                table: "NotificationModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationModel_PassengerId1",
                table: "NotificationModel",
                column: "PassengerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationModel_passengers_PassengerId1",
                table: "NotificationModel",
                column: "PassengerId1",
                principalTable: "passengers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationModel_passengers_PassengerId1",
                table: "NotificationModel");

            migrationBuilder.DropIndex(
                name: "IX_NotificationModel_PassengerId1",
                table: "NotificationModel");

            migrationBuilder.DropColumn(
                name: "PassengerId1",
                table: "NotificationModel");

            }
    }
}
