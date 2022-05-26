using MAS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAS.Infrastructure.Data.Configuration
{
    public class MasUserConfiguration : IEntityTypeConfiguration<MasUser>
    {
        public void Configure(EntityTypeBuilder<MasUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnType("nvarchar(100)");
            builder.Property(x => x.CreateDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.UpdateDate).HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.IdentityId).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.Email).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.Introduce).HasColumnType("nvarchar(1000)").IsRequired(false);
            builder.Property(x => x.Avatar).HasColumnType("nvarchar(MAX)").IsRequired();
            builder.Property(x => x.MeetUrl).HasColumnType("nvarchar(200)").IsRequired(false);
            builder.Property(x => x.IsMentor).HasColumnType("bit").IsRequired();
            builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();
        }
    }
}
