﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations;

public partial class ModifyAppointment : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "StartTime",
            table: "Appointments",
            type: "datetime",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "datetime",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "FinishTime",
            table: "Appointments",
            type: "datetime",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "datetime",
            oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "StartTime",
            table: "Appointments",
            type: "datetime",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime");

        migrationBuilder.AlterColumn<DateTime>(
            name: "FinishTime",
            table: "Appointments",
            type: "datetime",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime");
    }
}
