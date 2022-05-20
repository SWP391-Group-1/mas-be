using MAS.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<MasUser> MasUsers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<AppointmentSubject> AppointmentSubjects { get; set; }
        public DbSet<MentorSubject> MentorSubjects { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Appointment>()
            //    .HasOne<MasUser>(a => a.Student)
            //    .WithMany(st => st.Appointments)
            //    .HasForeignKey(st => st.StudentId);

            //builder.Entity<Appointment>()
            //    .HasOne<MasUser>(a => a.Mentor)
            //    .WithOne(m => m.Appointment)
            //    .HasForeignKey<Appointment>(a => a.MentorId);


            //builder.Entity<Appointment>()
            //   .HasOne<MasUser>(a => a.Mentor)
            //   .WithMany(m => m.Appointments)
            //   .HasForeignKey(a => a.MentorId);

        }
    }
}
