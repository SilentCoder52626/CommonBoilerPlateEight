using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityReviewMapping : IEntityTypeConfiguration<CelebrityReview>
    {
        public void Configure(EntityTypeBuilder<CelebrityReview> builder)
        {
            builder.HasKey(cr => cr.Id);
            builder.Property(cr => cr.Rating)
                .IsRequired()
                .HasColumnType("decimal(3,1)");
            builder.Property(cr => cr.ReviewText)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(a => a.ApprovedById);
            builder.Property(a => a.IsApprovedByAdmin).IsRequired().HasDefaultValue(false);

            // Relationships

            builder.HasOne(a => a.ApprovedBy).WithMany().HasForeignKey(a => a.ApprovedById).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(cr => cr.CelebrityAdvertisement)
                .WithMany(b => b.CelebrityReviews)
                .HasForeignKey(cr => cr.AdId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
