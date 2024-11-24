using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityToSocialLinkMapping : IEntityTypeConfiguration<CelebrityToSocialLink>
    {
        public void Configure(EntityTypeBuilder<CelebrityToSocialLink> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.CelebrityId).IsRequired();
            builder.Property(a => a.Url).IsRequired().HasMaxLength(350);
            builder.Property(a => a.Icon).HasMaxLength(400);
            builder.Property(a => a.Platform).HasMaxLength(100).HasConversion(
              v => v.ToString(),
              s => (SocialLinkEnum)Enum.Parse(typeof(SocialLinkEnum), s));
            builder.HasOne(a => a.Celebrity).WithMany(a => a.CelebrityToSocialLinks).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
