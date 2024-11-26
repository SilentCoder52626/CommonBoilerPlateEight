using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CartItemMapping : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {

            // Primary Key
            builder.HasKey(ci => ci.Id);

            // Properties
            builder.Property(ci => ci.CompanyName).HasMaxLength(200);
            builder.Property(ci => ci.ContactPerson).HasMaxLength(400);
            builder.Property(ci => ci.ManagerPhone).HasMaxLength(20);
            builder.Property(ci => ci.AdDate).IsRequired();
            builder.Property(ci => ci.AdPrice).IsRequired().HasColumnType("decimal(18,3)");
            builder.Property(a => a.DeliveryType).IsRequired().HasMaxLength(100).HasConversion(
               v => v.ToString(),
               s => (DeliveryTypeEnum)Enum.Parse(typeof(DeliveryTypeEnum), s));

            // Relationships
           
            builder.HasOne(b => b.Celebrity)
                  .WithMany(c => c.CartItems)
                  .HasForeignKey(b => b.CelebrityId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ci => ci.CartItemQuestions)
                .WithOne(q => q.CartItem)
                .HasForeignKey(q => q.CartItemId)
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
        }
    }
}
