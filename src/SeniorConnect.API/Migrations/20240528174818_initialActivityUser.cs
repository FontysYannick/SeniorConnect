using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorConnect.API.Migrations
{
    /// <inheritdoc />
    public partial class initialActivityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_OrganizerId",
                table: "Activities");

            migrationBuilder.CreateTable(
                name: "ActivityUsers",
                columns: table => new
                {
                    ActivityUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityUsers", x => x.ActivityUserId);
                    table.ForeignKey(
                        name: "FK_ActivityUsers_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityUsers_ActivityId",
                table: "ActivityUsers",
                column: "ActivityId");

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

            migrationBuilder.DropTable(
                name: "ActivityUsers");

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
