using fastfood_orders.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace fastfood_orders.Infra.SqlServer.Configuration;

[ExcludeFromCodeCoverage]
public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(c => c.Id).HasName("OrderId");
        builder.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
        builder.Property(c => c.Amount).HasColumnName("Amount").HasColumnType("decimal(18, 2)");
        builder.Property(c => c.CreationDate).HasColumnName("CreationDate");
        builder.Property(c => c.Status).HasColumnName("Status");

        builder.HasMany(c => c.OrderedItems)
        .WithOne(u => u.Order)
        .HasForeignKey(y => y.OrderId);
    }
}
