using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations
{
    public partial class RemoverelationshipappointmentandmentorMasUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_MasUsers_MentorId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MentorId",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "MentorId",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MentorId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MentorId",
                table: "Appointments",
                column: "MentorId",
                unique: true,
                filter: "[MentorId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_MasUsers_MentorId",
                table: "Appointments",
                column: "MentorId",
                principalTable: "MasUsers",
                principalColumn: "Id");
        }
    }
}
