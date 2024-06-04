using LibraryManagement.Domain.Library.BookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configs;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Title).IsRequired();
        builder.Property(r => r.Quantity).IsRequired();
        builder.Property(r => r.Price).IsRequired();
        builder.Property(r => r.PublicationDate).IsRequired();
        builder.Property(r => r.Genre).IsRequired();
    }
}
