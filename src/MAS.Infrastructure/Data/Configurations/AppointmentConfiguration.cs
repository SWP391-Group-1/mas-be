using MAS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAS.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("nvarchar(100)");
        builder.Property(x => x.CreateDate).HasColumnType("datetime").IsRequired();
        builder.Property(x => x.UpdateDate).HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.CreatorId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.MentorId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.SlotId).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.IsApprove).HasColumnType("bit").IsRequired();
        builder.HasOne(a => a.Slot)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.SlotId);
        builder.Property(x => x.StartTime).HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.FinishTime).HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.MentorDescription).HasColumnType("nvarchar(1000)").IsRequired(false);
    }
}
