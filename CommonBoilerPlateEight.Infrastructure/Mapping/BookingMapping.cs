using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class BookingMapping : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {

            builder.HasKey(e => e.Id);
            builder.Property(b => b.TrackingId).IsRequired().HasMaxLength(20);
            builder.HasIndex(b => b.TrackingId).IsUnique();

            builder.Property(a => a.Status).IsRequired().HasMaxLength(100).HasConversion(
              v => v.ToString(),
              s => (BookingStatusEnum)Enum.Parse(typeof(BookingStatusEnum), s));


            // Relationships
            builder.HasMany(b => b.CelebrityAdvertisements)
                .WithOne(ca => ca.Booking)
                .HasForeignKey(ca => ca.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(b => b.BookingHistories)
                .WithOne(bh => bh.Booking)
                .HasForeignKey(ca => ca.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            

            builder.Property(b => b.IsCreatedByAdmin)
                              .IsRequired();

            builder.HasOne(b => b.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(b => b.CreatedBy)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
