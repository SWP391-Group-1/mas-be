﻿// <auto-generated />
using System;
using MAS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MAS.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220714140127_Modify Appointment")]
    partial class ModifyAppointment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MAS.Core.Entities.Appointment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("BriefProblem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsApprove")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsPassed")
                        .HasColumnType("bit");

                    b.Property<string>("MentorDescription")
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("MentorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SlotId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("SlotId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("MAS.Core.Entities.Major", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Majors");
                });

            modelBuilder.Entity("MAS.Core.Entities.MasUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Introduce")
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsMentor")
                        .HasColumnType("bit");

                    b.Property<string>("MeetUrl")
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NumOfAppointment")
                        .HasColumnType("int");

                    b.Property<int>("NumOfRate")
                        .HasColumnType("int");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("MasUsers");
                });

            modelBuilder.Entity("MAS.Core.Entities.MentorSubject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("BriefInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("MentorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("MentorId");

                    b.HasIndex("SubjectId");

                    b.ToTable("MentorSubjects");
                });

            modelBuilder.Entity("MAS.Core.Entities.Question", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("AppointmentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("QuestionContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("MAS.Core.Entities.Rating", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AppointmentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsApprove")
                        .HasColumnType("bit");

                    b.Property<string>("MentorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Vote")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("MAS.Core.Entities.Slot", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("IsPassed")
                        .HasColumnType("bit");

                    b.Property<string>("MentorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("MentorId");

                    b.ToTable("Slots");
                });

            modelBuilder.Entity("MAS.Core.Entities.SlotSubject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("SlotId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SubjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("SlotId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SlotSubjects");
                });

            modelBuilder.Entity("MAS.Core.Entities.Subject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("MajorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("MajorId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MAS.Core.Entities.Appointment", b =>
                {
                    b.HasOne("MAS.Core.Entities.Slot", "Slot")
                        .WithMany("Appointments")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("MAS.Core.Entities.MentorSubject", b =>
                {
                    b.HasOne("MAS.Core.Entities.MasUser", "Mentor")
                        .WithMany("MentorSubjects")
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MAS.Core.Entities.Subject", "Subject")
                        .WithMany("MentorSubjects")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mentor");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("MAS.Core.Entities.Question", b =>
                {
                    b.HasOne("MAS.Core.Entities.Appointment", "Appointment")
                        .WithMany("Questions")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("MAS.Core.Entities.Rating", b =>
                {
                    b.HasOne("MAS.Core.Entities.Appointment", "Appointment")
                        .WithMany("Ratings")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");
                });

            modelBuilder.Entity("MAS.Core.Entities.Slot", b =>
                {
                    b.HasOne("MAS.Core.Entities.MasUser", "Mentor")
                        .WithMany("Slots")
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mentor");
                });

            modelBuilder.Entity("MAS.Core.Entities.SlotSubject", b =>
                {
                    b.HasOne("MAS.Core.Entities.Slot", "Slot")
                        .WithMany("SlotSubjects")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MAS.Core.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slot");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("MAS.Core.Entities.Subject", b =>
                {
                    b.HasOne("MAS.Core.Entities.Major", "Major")
                        .WithMany("Subjects")
                        .HasForeignKey("MajorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Major");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MAS.Core.Entities.Appointment", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("MAS.Core.Entities.Major", b =>
                {
                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("MAS.Core.Entities.MasUser", b =>
                {
                    b.Navigation("MentorSubjects");

                    b.Navigation("Slots");
                });

            modelBuilder.Entity("MAS.Core.Entities.Slot", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("SlotSubjects");
                });

            modelBuilder.Entity("MAS.Core.Entities.Subject", b =>
                {
                    b.Navigation("MentorSubjects");
                });
#pragma warning restore 612, 618
        }
    }
}
