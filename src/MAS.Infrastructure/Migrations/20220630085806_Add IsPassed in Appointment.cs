using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations;

public partial class AddIsPassedinAppointment : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsPassed",
            table: "Appointments",
            type: "bit",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsPassed",
            table: "Appointments");
    }
}
