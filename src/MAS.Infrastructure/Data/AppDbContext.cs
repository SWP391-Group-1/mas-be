using MAS.Core.Entities;
using MAS.Infrastructure.Data.Configurations;
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
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<AppointmentSubject> AppointmentSubjects { get; set; }
        public DbSet<MentorSubject> MentorSubjects { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(MasUserConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(SlotConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(AppointmentConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(RatingConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(MajorConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(SubjectConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(AppointmentSubjectConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(MentorSubjectConfiguration).Assembly);
            builder.ApplyConfigurationsFromAssembly(typeof(QuestionConfiguration).Assembly);
        }
    }
}