using fastfood_orders.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_orders.Infra.SqlServer.Configuration
{

    [ExcludeFromCodeCoverage]
    public class OrderedItemConfiguration : IEntityTypeConfiguration<OrderedItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderedItemEntity> builder)
        {
            builder.ToTable("OrderedItem");
            builder.HasKey(c => c.Id).HasName("OrderedItemId");
            builder.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(c => c.ProductId).HasColumnName("ProductId");
            builder.Property(c => c.OrderId).HasColumnName("OrderId");
            builder.Property(c => c.Quantity).HasColumnName("Quantity");

            builder.HasOne(c => c.Order)
            .WithMany(u => u.OrderedItems)
            .HasForeignKey(u => u.OrderId);
        }
    }
}