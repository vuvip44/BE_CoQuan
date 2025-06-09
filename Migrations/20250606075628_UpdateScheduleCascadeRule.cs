using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lich.api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleCascadeRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Users_ApprovedById",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Users_CreatedById",
                table: "Schedules");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Users_ApprovedById",
                table: "Schedules",
                column: "ApprovedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Users_CreatedById",
                table: "Schedules",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Users_ApprovedById",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Users_CreatedById",
                table: "Schedules");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Users_ApprovedById",
                table: "Schedules",
                column: "ApprovedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Users_CreatedById",
                table: "Schedules",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
