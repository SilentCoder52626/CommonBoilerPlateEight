using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityAdHistoryMap : IEntityTypeConfiguration<CelebrityAdHistory>
    {
        public void Configure(EntityTypeBuilder<CelebrityAdHistory> builder)
        {
            builder.HasKey(cah => cah.Id);

            builder.Property(cah => cah.Comment)
                   .HasMaxLength(500);

            builder.Property(cah => cah.Status).IsRequired().HasMaxLength(100).HasConversion(
              v => v.ToString(),
              s => (BookingStatusEnum)Enum.Parse(typeof(BookingStatusEnum), s));

            builder.HasOne(cah => cah.CelebrityAd)
                   .WithMany(ca => ca.CelebrityAdHistories)
                   .HasForeignKey(cah => cah.AdId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
