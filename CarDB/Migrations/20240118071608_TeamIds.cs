using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDB.Migrations
{
    /// <inheritdoc />
    public partial class TeamIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Team1Id",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Team2Id",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team1Id",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Team2Id",
                table: "Tournaments");
        }
    }
}
