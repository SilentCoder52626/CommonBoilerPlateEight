//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using CommonBoilerPlateEight.Domain.Entity;
//using CommonBoilerPlateEight.Domain.Enums;

//namespace CommonBoilerPlateEight.Infrastructure.Mapping
//{
//    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
//    {
//        public void Configure(EntityTypeBuilder<Voucher> builder)
//        {
//            builder.HasKey(v => v.Id);

//            builder.Property(v => v.DiscountPercent)
//                   .IsRequired()
//                   .HasMaxLength(3);

//            builder.Property(v => v.VoucherType)
//                   .IsRequired()
//                   .HasConversion(
//                v => v.ToString(),
//                s => (VoucherTypeEnum)Enum.Parse(typeof(VoucherTypeEnum), s));

//            builder.Property(v => v.ExpiryDate)
//                   .IsRequired();

//            builder.Property(v => v.PromoCode)
//                   .IsRequired()
//                   .HasMaxLength(100);

//            builder.HasOne(v => v.Customer)
//                   .WithMany(c => c.Vouchers)
//                   .HasForeignKey(v => v.CustomerId);
//        }
//    }
//}
