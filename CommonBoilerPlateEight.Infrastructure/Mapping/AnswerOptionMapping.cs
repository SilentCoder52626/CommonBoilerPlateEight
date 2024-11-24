using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class AnswerOptionMapping : IEntityTypeConfiguration<AnswerOption>
    {
        public void Configure(EntityTypeBuilder<AnswerOption> builder)
        {
          builder.HasKey(a=>a.Id);
            builder.Property(a => a.QuestionSettingId).IsRequired();
            builder.Property(a=>a.OptionText).IsRequired();
            builder.HasOne(a => a.QuestionSetting).WithMany(a => a.AnswerOptions).HasForeignKey(a => a.QuestionSettingId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
