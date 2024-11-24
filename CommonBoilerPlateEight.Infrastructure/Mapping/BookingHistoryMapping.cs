using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class BookingHistoryMap : IEntityTypeConfiguration<BookingHistory>
    {
        public void Configure(EntityTypeBuilder<BookingHistory> builder)
        {

            builder.HasKey(bh => bh.Id);

            builder.Property(bh => bh.Comment)
                   .HasMaxLength(500);

            builder.Property(bh => bh.Status).IsRequired().HasMaxLength(100).HasConversion(
               v => v.ToString(),
               s => (BookingStatusEnum)Enum.Parse(typeof(BookingStatusEnum), s));

            builder.HasOne(bh => bh.Booking)
                   .WithMany(b => b.BookingHistories)
                   .HasForeignKey(bh => bh.BookingId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
