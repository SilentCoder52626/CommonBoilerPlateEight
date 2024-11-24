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
    public class CountryMapping : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a=>a.Name).IsRequired().HasMaxLength(200);
            builder.Property(a=>a.Code).IsRequired().HasMaxLength(200);
            builder.Property(a=>a.FlagCode).IsRequired().HasMaxLength(200);
            builder.Property(a=>a.DialCode).IsRequired().HasMaxLength(200);
        }
    }
}
