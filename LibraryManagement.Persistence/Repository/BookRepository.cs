using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Specifications.Books;
using LibraryManagement.Persistence.Entity_Framework_Core;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repository;

public class BookRepository : RepositoryImpl<Book, int>, IBookRepository
{
    public BookRepository(AppDbContext appDbContext, IAppLogger<BookRepository> logger) : base(appDbContext, logger)
    {
    }

    public async Task<List<Review>> GetBookReviews(int bookId)
    {
        try
        {
            var reviews =
                await AppDbContext
                    .Reviews
                    .Where(r => r.BookId == bookId)
                    .ToListAsync();

            return reviews;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetBookReviews", ex.Message);
            return new();
        }
    }

    public async Task<List<Author>> GetAuthors(AuthorListByIdQuerySpec authorListByIdQuerySpec)
    {
        try
        {
            if (authorListByIdQuerySpec.Criteria is not null)
            {
                var specificationResult = AppDbContext.Authors.Where(authorListByIdQuerySpec.Criteria);
                return await specificationResult.ToListAsync();
            }

            throw new ArgumentException();
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAuthors", ex.Message);
            return new();
        }
    }

    public override async Task<Book?> GetByIdAsync(int id)
    {
        try
        {
            return await AppDbContext
                .Books
                .Include(x => x.Authors)
                .Include(x => x.Reviews)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAuthors", ex.Message);
            return null;
        }
    }
}