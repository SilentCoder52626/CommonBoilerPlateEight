using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;


namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityAdvertisementQuestionMapping : IEntityTypeConfiguration<CelebrityAdvertismentQuestion>
    {
        public void Configure(EntityTypeBuilder<CelebrityAdvertismentQuestion> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.TextAnswer)
                .HasMaxLength(1000);
            builder.Property(q => q.SelectedOptionId);
            builder.Property(q => q.DateAnswer);
            builder.Property(q => q.NumberAnswer).HasColumnType("decimal(18,3)");

            // Relationships
            builder.HasOne(q => q.CelebrityAdvertisement)
                .WithMany(ca => ca.CelebrityAdvertismentQuestions)
                .HasForeignKey(q => q.AdId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(q => q.QuestionSetting)
                .WithMany()
                .HasForeignKey(q => q.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
