using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityMapping : IEntityTypeConfiguration<Celebrity>
    {
        public void Configure(EntityTypeBuilder<Celebrity> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.FullName).HasMaxLength(400);
            builder.Property(a => a.Email).HasMaxLength(400);
            builder.HasIndex(a => a.Email).IsUnique().HasFilter("[DeletedDate] IS NULL");
            builder.Property(a => a.CountryId);
            builder.Property(a => a.MobileNumber).HasMaxLength(400);
            builder.HasIndex(a => new { a.CountryId, a.MobileNumber })
        .IsUnique().HasFilter("[DeletedDate] IS NULL");
            builder.Property(a => a.ProfilePictureURL).HasMaxLength(400);
            builder.Property(a => a.TimeToCall);
            builder.Property(a => a.Password);
            builder.Property(a => a.PricePerPost).HasPrecision(18, 3).IsRequired().HasDefaultValue(0);
            builder.Property(a => a.PricePerEvent).HasPrecision(18, 3).IsRequired().HasDefaultValue(0);
            builder.Property(a => a.PricePerDelivery).HasPrecision(18, 3).IsRequired().HasDefaultValue(0);
            builder.Property(a => a.PriceRange).IsRequired().HasDefaultValue(string.Empty).HasMaxLength(200);
            builder.Property(a => a.Description);
            builder.Property(a => a.DeviceId);
            builder.Property(a => a.OTP).HasMaxLength(100);
            builder.Property(a => a.OTPCreatedOn);
            builder.Property(a => a.ApprovedById);
            builder.Property(a => a.RejectedById);
            builder.Property(a => a.RejectionRemarks);
            builder.Property(a => a.IsCreatedByAdmin).IsRequired().HasDefaultValue(false);
            builder.Property(a => a.IsOnline).IsRequired().HasDefaultValue(true);
            builder.Property(a => a.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(a => a.Status).IsRequired().HasMaxLength(100).HasConversion(
               v => v.ToString(),
               s => (StatusTypeEnum)Enum.Parse(typeof(StatusTypeEnum), s));
            builder.Property(a => a.AuthenticationType).HasMaxLength(100).HasConversion(
               v => v.ToString(),
               s => (AuthenticationTypeEnum)Enum.Parse(typeof(AuthenticationTypeEnum), s));
            builder.Property(a => a.Gender).HasMaxLength(100).HasConversion(
               v => v.ToString(),
               s => (GenderTypeEnum)Enum.Parse(typeof(GenderTypeEnum), s));
            builder.HasOne(a => a.CreatedByUser).WithMany().HasForeignKey(a => a.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.Country).WithMany().HasForeignKey(a => a.CountryId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.ApprovedByUser).WithMany().HasForeignKey(a => a.ApprovedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.RejectedByUser).WithMany().HasForeignKey(a => a.RejectedById).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(a => a.CelebrityToTypes).WithOne(a => a.Celebrity).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(a => a.CelebrityToSocialLinks).WithOne(a => a.Celebrity).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(a => a.CelebrityToAttachments).WithOne(a => a.Celebrity).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(a => a.CelebritySchedules).WithOne(a => a.Celebrity).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(a => a.CelebrityLocations).WithOne(a => a.Celebrity).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(a => a.CartItems).WithOne(a => a.Celebrity).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(c => c.CelebrityAdvertisements).WithOne(e => e.Celebrity).HasForeignKey(e => e.CelebrityId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
