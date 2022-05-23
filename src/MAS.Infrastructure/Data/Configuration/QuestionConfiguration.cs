using MAS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAS.Infrastructure.Data.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("nvarchar(100)");
            builder.Property(x => x.CreateDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.UpdateDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.SubjectId).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.AppointmentId).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.QuestionContent).HasColumnType("nvarchar(500)").IsRequired();
            builder.Property(x => x.Answer).HasColumnType("nvarchar(MAX)").IsRequired(false);
        }
    }
}
