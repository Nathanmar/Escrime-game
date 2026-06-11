using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EscrimeGame.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CharacterClass",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterClass",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Players");
        }
    }
}
