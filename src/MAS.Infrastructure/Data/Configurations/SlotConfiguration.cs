using MAS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAS.Infrastructure.Data.Configurations;

public class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("nvarchar(100)");
        builder.Property(x => x.CreateDate).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.UpdateDate).HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.MentorId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.StartTime).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.FinishTime).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.IsPassed).HasColumnType("bit").IsRequired(false);
        builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();
    }
}
