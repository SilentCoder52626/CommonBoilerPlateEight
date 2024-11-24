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
    public class CelebrityLocationMapping : IEntityTypeConfiguration<CelebrityLocation>
    {
        public void Configure(EntityTypeBuilder<CelebrityLocation> builder)
        {
            builder
            .HasKey(x => x.Id);

            builder.Property(l => l.Area)
                .HasMaxLength(400)
                .IsRequired(false);

            builder.Property(l => l.Governorate)
                .HasMaxLength(400)
                .IsRequired(false);

            builder.Property(l => l.Block)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(l => l.Street)
                .HasMaxLength(400)
                .IsRequired(false);

            builder.Property(l => l.GooglePlusCode)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(l => l.Note)
              .IsRequired(false);

            builder.Property(l => l.Latitude).IsRequired().HasPrecision(18, 15);

            builder.Property(l => l.Longitude).IsRequired().HasPrecision(18, 15);
            builder.Property(l => l.FullAddress).HasMaxLength(500).IsRequired(true);
            builder.Property(l => l.CelebrityId).IsRequired(true);
            builder.HasOne(a=>a.Celebrity).WithMany(a=>a.CelebrityLocations).HasForeignKey(a=>a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(e => e.Location)
                .HasComputedColumnSql("Geography::Point(Latitude, Longitude, 4326)", stored: true);
        }
    }
}
