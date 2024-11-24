using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityToTypeMapping : IEntityTypeConfiguration<CelebrityToType>
    {
        public void Configure(EntityTypeBuilder<CelebrityToType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.CelebrityId).IsRequired();
            builder.Property(a => a.CelebrityTypeId).IsRequired();
            builder.HasOne(a => a.Celebrity).WithMany(a => a.CelebrityToTypes).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.CelebrityType).WithMany().HasForeignKey(a => a.CelebrityTypeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
