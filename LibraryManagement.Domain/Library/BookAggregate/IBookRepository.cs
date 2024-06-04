using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Specifications.Books;

namespace LibraryManagement.Domain.Library.BookAggregate;

public interface IBookRepository : IRepository<Book, int>
{
    Task<List<Review>> GetBookReviews(int bookId);
    Task<List<Author>> GetAuthors(AuthorListByIdQuerySpec authorListByIdQuerySpec);
}