using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class QuestionSettingMapping : IEntityTypeConfiguration<QuestionSetting>
    {
        public void Configure(EntityTypeBuilder<QuestionSetting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.Question).IsRequired();
            builder.Property(a => a.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(a => a.AnswerType).IsRequired().HasMaxLength(100).HasConversion(
              v => v.ToString(),
              s => (AnswerTypeEnum)Enum.Parse(typeof(AnswerTypeEnum), s));
            builder.Property(a => a.DeliveryType).IsRequired().HasMaxLength(100).HasConversion(
               v => v.ToString(),
               s => (DeliveryTypeEnum)Enum.Parse(typeof(DeliveryTypeEnum), s));
            builder.HasMany(a => a.AnswerOptions).WithOne(a => a.QuestionSetting).HasForeignKey(a => a.QuestionSettingId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
