using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashierDB.Model.Configure
{
    class ItemConfiguration : IEntityTypeConfiguration<ItemDbDto>
    {
        public void Configure(EntityTypeBuilder<ItemDbDto> builder)
        {
            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey("Barcode")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
