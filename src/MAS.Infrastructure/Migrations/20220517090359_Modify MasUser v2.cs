using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations
{
    public partial class ModifyMasUserv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "MasUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityId",
                table: "MasUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "MasUsers");

            migrationBuilder.DropColumn(
                name: "IdentityId",
                table: "MasUsers");
        }
    }
}
