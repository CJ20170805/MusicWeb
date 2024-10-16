using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForAdminUserFromNotificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForAdmin",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsForUser",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForAdmin",
                table: "Notifications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsForUser",
                table: "Notifications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
