using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;


namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityToAttachmentMapping : IEntityTypeConfiguration<CelebrityToAttachment>
    {
        public void Configure(EntityTypeBuilder<CelebrityToAttachment> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.CelebrityId).IsRequired();
            builder.Property(a => a.FileName).IsRequired().HasMaxLength(400);
            builder.Property(a => a.FilePath).IsRequired().HasMaxLength(500);
            builder.Property(a => a.ContentType).IsRequired().HasMaxLength(400);
            builder.Property(a => a.AttachmentType).HasMaxLength(100).HasConversion(
             v => v.ToString(),
             s => (CelebrityAttachmentTypeEnum)Enum.Parse(typeof(CelebrityAttachmentTypeEnum), s));
            builder.HasOne(a => a.Celebrity).WithMany(a => a.CelebrityToAttachments).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
