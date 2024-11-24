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
    public class SettingMapping : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Key).IsRequired().HasMaxLength(500);
            builder.Property(a => a.Value).IsRequired();
            builder.HasIndex(a => a.Key).IsUnique();
        }
    }
}
