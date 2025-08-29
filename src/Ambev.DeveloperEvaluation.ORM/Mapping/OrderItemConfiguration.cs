using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(i => i.OrderId).HasColumnType("uuid").IsRequired();
        builder.Property(i => i.Description).HasMaxLength(200).IsRequired();
        builder.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(i => i.Quantity).IsRequired();
    }
}