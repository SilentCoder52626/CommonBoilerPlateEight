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
    public class CelebrityScheduleMapping : IEntityTypeConfiguration<CelebritySchedule>
    {
        public void Configure(EntityTypeBuilder<CelebritySchedule> builder)
        {
            builder.HasKey(a=>a.Id);
            builder.Property(a => a.Date).IsRequired();
            builder.Property(a => a.From).IsRequired();
            builder.Property(a => a.To).IsRequired();
            builder.Property(a => a.CelebrityId).IsRequired();
            builder.HasOne(a => a.Celebrity).WithMany(a => a.CelebritySchedules).HasForeignKey(a => a.CelebrityId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.CreatedByUser).WithMany().HasForeignKey(a => a.CreatedBy).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
