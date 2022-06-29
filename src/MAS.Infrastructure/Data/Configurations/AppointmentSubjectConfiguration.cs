using MAS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAS.Infrastructure.Data.Configurations;

public class AppointmentSubjectConfiguration : IEntityTypeConfiguration<AppointmentSubject>
{
    public void Configure(EntityTypeBuilder<AppointmentSubject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("nvarchar(100)");
        builder.Property(x => x.CreateDate).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.UpdateDate).HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.AppointmentId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.SubjectId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.BriefProblem).HasColumnType("nvarchar(500)").IsRequired();
        builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();
    }
}
