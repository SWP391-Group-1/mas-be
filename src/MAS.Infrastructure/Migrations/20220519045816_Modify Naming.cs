using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations
{
    public partial class ModifyNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Slots",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Slots",
                newName: "FinishTime");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Questions",
                newName: "QuestionContent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Slots",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "FinishTime",
                table: "Slots",
                newName: "End");

            migrationBuilder.RenameColumn(
                name: "QuestionContent",
                table: "Questions",
                newName: "Content");
        }
    }
}
