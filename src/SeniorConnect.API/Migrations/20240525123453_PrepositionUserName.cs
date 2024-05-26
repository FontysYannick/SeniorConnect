using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class PrepositionUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Preposition",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preposition",
                table: "Users");
        }
    }
}
