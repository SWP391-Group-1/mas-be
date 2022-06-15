using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations;

public partial class AddIsActivetoBaseEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Subjects",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Slots",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Questions",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "MentorSubjects",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Majors",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "AppointmentSubjects",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "Appointments",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Subjects");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Slots");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "MentorSubjects");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Majors");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "AppointmentSubjects");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "Appointments");
    }
}
