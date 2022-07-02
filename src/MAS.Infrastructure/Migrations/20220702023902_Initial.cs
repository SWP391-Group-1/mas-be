using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Infrastructure.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUsers",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Majors",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Code = table.Column<string>(type: "nvarchar(10)", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Majors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MasUsers",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                IdentityId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Avatar = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                Introduce = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                Rate = table.Column<double>(type: "float", nullable: false),
                NumOfRate = table.Column<int>(type: "int", nullable: false),
                NumOfAppointment = table.Column<int>(type: "int", nullable: false),
                IsMentor = table.Column<bool>(type: "bit", nullable: true),
                MeetUrl = table.Column<string>(type: "nvarchar(200)", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MasUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Subjects",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                MajorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Code = table.Column<string>(type: "nvarchar(10)", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subjects", x => x.Id);
                table.ForeignKey(
                    name: "FK_Subjects_Majors_MajorId",
                    column: x => x.MajorId,
                    principalTable: "Majors",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Slots",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                MentorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                FinishTime = table.Column<DateTime>(type: "datetime", nullable: false),
                IsPassed = table.Column<bool>(type: "bit", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Slots", x => x.Id);
                table.ForeignKey(
                    name: "FK_Slots_MasUsers_MentorId",
                    column: x => x.MentorId,
                    principalTable: "MasUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MentorSubjects",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                MentorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                SubjectId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                BriefInfo = table.Column<string>(type: "nvarchar(500)", nullable: false),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MentorSubjects", x => x.Id);
                table.ForeignKey(
                    name: "FK_MentorSubjects_MasUsers_MentorId",
                    column: x => x.MentorId,
                    principalTable: "MasUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MentorSubjects_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalTable: "Subjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Appointments",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                CreatorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                MentorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                SlotId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                BriefProblem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsApprove = table.Column<bool>(type: "bit", nullable: true),
                StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                MentorDescription = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                FinishTime = table.Column<DateTime>(type: "datetime", nullable: true),
                IsPassed = table.Column<bool>(type: "bit", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Appointments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Appointments_Slots_SlotId",
                    column: x => x.SlotId,
                    principalTable: "Slots",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SlotSubjects",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                SlotId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                SubjectId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SlotSubjects", x => x.Id);
                table.ForeignKey(
                    name: "FK_SlotSubjects_Slots_SlotId",
                    column: x => x.SlotId,
                    principalTable: "Slots",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SlotSubjects_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalTable: "Subjects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Questions",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                AppointmentId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                CreatorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                QuestionContent = table.Column<string>(type: "nvarchar(500)", nullable: false),
                Answer = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Questions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Questions_Appointments_AppointmentId",
                    column: x => x.AppointmentId,
                    principalTable: "Appointments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Ratings",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(100)", nullable: false),
                AppointmentId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                CreatorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                MentorId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Vote = table.Column<int>(type: "int", nullable: false),
                Comment = table.Column<string>(type: "nvarchar(500)", nullable: true),
                IsApprove = table.Column<bool>(type: "bit", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Ratings", x => x.Id);
                table.ForeignKey(
                    name: "FK_Ratings_Appointments_AppointmentId",
                    column: x => x.AppointmentId,
                    principalTable: "Appointments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Appointments_SlotId",
            table: "Appointments",
            column: "SlotId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "AspNetUsers",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_MentorSubjects_MentorId",
            table: "MentorSubjects",
            column: "MentorId");

        migrationBuilder.CreateIndex(
            name: "IX_MentorSubjects_SubjectId",
            table: "MentorSubjects",
            column: "SubjectId");

        migrationBuilder.CreateIndex(
            name: "IX_Questions_AppointmentId",
            table: "Questions",
            column: "AppointmentId");

        migrationBuilder.CreateIndex(
            name: "IX_Ratings_AppointmentId",
            table: "Ratings",
            column: "AppointmentId");

        migrationBuilder.CreateIndex(
            name: "IX_Slots_MentorId",
            table: "Slots",
            column: "MentorId");

        migrationBuilder.CreateIndex(
            name: "IX_SlotSubjects_SlotId",
            table: "SlotSubjects",
            column: "SlotId");

        migrationBuilder.CreateIndex(
            name: "IX_SlotSubjects_SubjectId",
            table: "SlotSubjects",
            column: "SubjectId");

        migrationBuilder.CreateIndex(
            name: "IX_Subjects_MajorId",
            table: "Subjects",
            column: "MajorId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "MentorSubjects");

        migrationBuilder.DropTable(
            name: "Questions");

        migrationBuilder.DropTable(
            name: "Ratings");

        migrationBuilder.DropTable(
            name: "SlotSubjects");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "AspNetUsers");

        migrationBuilder.DropTable(
            name: "Appointments");

        migrationBuilder.DropTable(
            name: "Subjects");

        migrationBuilder.DropTable(
            name: "Slots");

        migrationBuilder.DropTable(
            name: "Majors");

        migrationBuilder.DropTable(
            name: "MasUsers");
    }
}
