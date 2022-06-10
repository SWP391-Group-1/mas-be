using MAS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAS.Infrastructure.Data.Configurations;

public class MentorSubjectConfiguration : IEntityTypeConfiguration<MentorSubject>
{
    public void Configure(EntityTypeBuilder<MentorSubject> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("nvarchar(100)");
        builder.Property(x => x.CreateDate).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.UpdateDate).HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.MentorId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.SubjectId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.BriefInfo).HasColumnType("nvarchar(500)").IsRequired();
    }
}
