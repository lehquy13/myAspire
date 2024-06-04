using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configs;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderDetailGuid.Create(value)
            );
        builder.Property(r => r.Quantity).IsRequired();
        builder.Property(r => r.UnitPrice).IsRequired();

        //Mark OrderId as a foreign key for OrderItem
        builder.Property(r => r.OrderId)
            .HasColumnName("OrderId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderGuid.Create(value)
            );
    }
}
