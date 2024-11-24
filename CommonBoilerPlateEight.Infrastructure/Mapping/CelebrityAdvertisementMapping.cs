using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;


namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityAdvertisementMapping : IEntityTypeConfiguration<CelebrityAdvertisement>
    {
        public void Configure(EntityTypeBuilder<CelebrityAdvertisement> builder)
        {


            builder.HasKey(ca => ca.Id);

            builder.Property(a => a.TrackingId).IsRequired().HasMaxLength(20);
            builder.HasIndex(a => a.TrackingId).IsUnique();

            builder.Property(ca => ca.CompanyName)
            .IsRequired()
            .HasMaxLength(200);
            builder.Property(ca => ca.ContactPerson)
                .IsRequired()
                .HasMaxLength(400);
            builder.Property(ca => ca.ManagerPhone)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(ca => ca.AdDate)
                .IsRequired();
            builder.Property(ca => ca.AdPrice)
               .IsRequired().HasColumnType("decimal(18,3)");

            builder.Property(a => a.Status).IsRequired().HasMaxLength(100).HasConversion(
              v => v.ToString(),
              s => (BookingStatusEnum)Enum.Parse(typeof(BookingStatusEnum), s));

            builder.Property(a => a.DeliveryType).IsRequired().HasMaxLength(100).HasConversion(
               v => v.ToString(),
               s => (DeliveryTypeEnum)Enum.Parse(typeof(DeliveryTypeEnum), s));

            // Relationships
            builder.HasOne(ca => ca.Booking)
             .WithMany(b => b.CelebrityAdvertisements)
             .HasForeignKey(ca => ca.BookingId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Celebrity)
                   .WithMany(c => c.CelebrityAdvertisements)
                   .HasForeignKey(b => b.CelebrityId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Customer)
               .WithMany(b => b.CelebrityAdvertisements)
               .HasForeignKey(b => b.CustomerId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(b => b.CelebrityReviews)
                   .WithOne(r => r.CelebrityAdvertisement)
                   .HasForeignKey(r => r.AdId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ca => ca.Country)
                .WithMany()
                .HasForeignKey(ca => ca.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ca => ca.CompanyType)
                .WithMany()
                .HasForeignKey(ca => ca.CompanyTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ca => ca.CelebritySchedule)
                .WithMany()
                .HasForeignKey(ca => ca.CelebrityScheduleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ca => ca.CelebrityAdvertismentQuestions)
                .WithOne(q => q.CelebrityAdvertisement)
                .HasForeignKey(q => q.AdId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ca => ca.CelebrityAdHistories)
                .WithOne(ah => ah.CelebrityAd)
                .HasForeignKey(ah => ah.AdId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
