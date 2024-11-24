using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CelebrityTypeMapping : IEntityTypeConfiguration<CelebrityType>
    {
        public void Configure(EntityTypeBuilder<CelebrityType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(350);
            builder.HasIndex(a => a.Name).IsUnique();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.CreatedBy).IsRequired();
            builder.HasOne(a => a.CreatedByUser).WithMany().HasForeignKey(a=>a.CreatedBy).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
