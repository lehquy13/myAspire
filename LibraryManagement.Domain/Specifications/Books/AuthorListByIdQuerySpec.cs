using LibraryManagement.Domain.Library.BookAggregate;

namespace LibraryManagement.Domain.Specifications.Books;

public class AuthorListByIdQuerySpec : SpecificationBase<Author>
{
    public AuthorListByIdQuerySpec(List<int> ints)
    {
        Criteria = author =>
            ints.Contains(author.Id);
    }
}