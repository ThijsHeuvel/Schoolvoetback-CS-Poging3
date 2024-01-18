using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDB.Migrations
{
    /// <inheritdoc />
    public partial class BetPaidOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidOut",
                table: "Tournaments");

            migrationBuilder.AddColumn<bool>(
                name: "PaidOut",
                table: "Bets",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidOut",
                table: "Bets");

            migrationBuilder.AddColumn<bool>(
                name: "PaidOut",
                table: "Tournaments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
