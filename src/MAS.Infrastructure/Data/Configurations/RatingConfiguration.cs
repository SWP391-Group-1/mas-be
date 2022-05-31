using MAS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAS.Infrastructure.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("nvarchar(100)");
            builder.Property(x => x.CreateDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.UpdateDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.AppointmentId).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.CreatorId).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.MentorId).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.Vote).HasColumnType("int").IsRequired();
            builder.Property(x => x.Comment).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(x => x.IsApprove).HasColumnType("bit").IsRequired(false);
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();
            //builder.HasOne(r => r.Mentor)
            //    .WithMany(u => u.Ratings)
            //    .HasForeignKey(r => r.MentorId);
            builder.HasOne(r => r.Appointment)
                .WithMany(a => a.Ratings)
                .HasForeignKey(r => r.AppointmentId);
        }
    }
}
