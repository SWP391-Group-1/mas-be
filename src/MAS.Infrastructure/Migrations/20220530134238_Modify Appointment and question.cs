using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations
{
    public partial class ModifyAppointmentandquestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_MasUsers_CreatorId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "MasUserId",
                table: "Questions",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_MasUserId",
                table: "Questions",
                column: "MasUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_MasUsers_MasUserId",
                table: "Questions",
                column: "MasUserId",
                principalTable: "MasUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_MasUsers_MasUserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_MasUserId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "MasUserId",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatorId",
                table: "Questions",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_MasUsers_CreatorId",
                table: "Questions",
                column: "CreatorId",
                principalTable: "MasUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
