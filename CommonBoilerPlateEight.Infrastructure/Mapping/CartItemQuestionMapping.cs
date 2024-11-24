using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CartItemQuestionMapping : IEntityTypeConfiguration<CartItemQuestion>
    {
        public void Configure(EntityTypeBuilder<CartItemQuestion> builder)
        {


            // Primary Key
            builder.HasKey(cq => cq.Id);

            // Properties
            builder.Property(cq => cq.TextAnswer).HasMaxLength(1000);
            builder.Property(cq => cq.SelectedOptionId);
            builder.Property(cq => cq.DateAnswer);
            builder.Property(cq => cq.NumberAnswer).HasColumnType("decimal(18,3)");

            // Relationships
            builder.HasOne(cq => cq.CartItem)
                .WithMany(ci => ci.CartItemQuestions)
                .HasForeignKey(cq => cq.CartItemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(cq => cq.QuestionSetting)
                .WithMany()
                .HasForeignKey(cq => cq.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
