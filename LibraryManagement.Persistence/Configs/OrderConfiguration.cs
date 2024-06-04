using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configs;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderGuid.Create(value)
            );
        builder.Property(r => r.UpdatedAt).IsRequired();
        builder.Property(r => r.CreatedAt).IsRequired();

        //Mark UserId as a foreign key for IdentityUser
        builder.Property(r => r.UserId)
            .HasColumnName("UserId")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );
    }
}
