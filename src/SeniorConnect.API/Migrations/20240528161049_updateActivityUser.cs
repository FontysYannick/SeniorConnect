using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class updateActivityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_OrganizerId",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityUsers",
                table: "ActivityUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityUsers",
                table: "ActivityUsers",
                column: "ActivityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityUsers_UserId",
                table: "ActivityUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_OrganizerId",
                table: "Activities",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_OrganizerId",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityUsers",
                table: "ActivityUsers");

            migrationBuilder.DropIndex(
                name: "IX_ActivityUsers_UserId",
                table: "ActivityUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityUsers",
                table: "ActivityUsers",
                columns: new[] { "UserId", "ActivityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_OrganizerId",
                table: "Activities",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
