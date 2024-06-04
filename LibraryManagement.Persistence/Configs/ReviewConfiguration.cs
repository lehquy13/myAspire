using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configs;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Title).IsRequired();
        builder.Property(r => r.Content).IsRequired();
        builder.Property(r => r.Rating).IsRequired();
        builder.Property(r => r.ReviewDate).IsRequired();
        builder.Property(r => r.IsLike).IsRequired();

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
