using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Utilities;

namespace LibraryManagement.Domain.Specifications.Books;

public sealed class BookListQuerySpec : GetListSpecificationBase<Book>
{
    public BookListQuerySpec(
        int pageIndex,
        int pageSize,
        string? title,
        string? authorName,
        string? genre,
        DateTime? publicationDate
    )
        : base(pageIndex, pageSize)
    {
        Criteria = book =>
            (string.IsNullOrEmpty(authorName) ||
             book.Authors.Any(x => x.Name.ToLower().Contains(authorName.ToLower()))) &&
            (string.IsNullOrEmpty(genre) || book.Genre == genre.ToEnum(Genre.Null)) &&
            (string.IsNullOrEmpty(title) || book.Title.ToLower().Contains(title.ToLower())) &&
            (publicationDate == null || book.PublicationDate >= publicationDate);

        AddInclude(book => book.Authors);
    }
}