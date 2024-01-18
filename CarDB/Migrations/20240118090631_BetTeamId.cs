using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDB.Migrations
{
    /// <inheritdoc />
    public partial class BetTeamId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BetTeamId",
                table: "Bets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetTeamId",
                table: "Bets");
        }
    }
}
