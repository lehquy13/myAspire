using LibraryManagement.Domain.Library.BookAggregate;

namespace LibraryManagement.Domain.Specifications.Books;

public sealed class BookListByIdQuerySpec : SpecificationBase<Book>
{
    public BookListByIdQuerySpec(List<int> bookIds)
    {
        Criteria = book1 => bookIds.Contains(book1.Id);
        AddInclude(book => book.Authors);
    }
}