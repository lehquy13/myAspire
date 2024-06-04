using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configs;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );
        builder.Property(r => r.Name).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();

        builder.OwnsOne(user => user.Address,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Country)
                    .HasColumnName("Country");
                navigationBuilder.Property(address => address.City)
                    .HasColumnName("City");
            });

        builder.HasMany(r => r.FavouriteBooks)
            .WithMany(b => b.BookFancier)
            .UsingEntity("FavouriteBooks");

        // builder.Metadata
        //     .FindNavigation("FavouriteBooks")?
        //     .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(r => r.WishLists)
            .WithMany()
            .UsingEntity("WishLists");
        //     .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
        //
        // builder.Metadata
        //     .FindNavigation("WishLists")?
        //     .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}