using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommonBoilerPlateEight.Domain.Entity;

namespace CommonBoilerPlateEight.Infrastructure.Mapping
{
    public class CustomerToCelebrityTypesMapping : IEntityTypeConfiguration<CustomerToCelebrityType>
    {
        public void Configure(EntityTypeBuilder<CustomerToCelebrityType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(a => a.CustomerId).IsRequired();
            builder.Property(a => a.CelebrityTypeId).IsRequired();
            builder.HasOne(a => a.Customer).WithMany(a => a.CustomerToCelebrityTypes).HasForeignKey(a => a.CustomerId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.CelebrityType).WithMany().HasForeignKey(a => a.CelebrityTypeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
