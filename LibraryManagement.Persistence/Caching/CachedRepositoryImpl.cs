using LazyCache;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Specifications;
using LibraryManagement.Domain.Specifications.Books;
using LibraryManagement.Persistence.Repository;

namespace LibraryManagement.Persistence.Caching;

public sealed class CachedBookRepositoryImpl : IBookRepository
{
    private readonly BookRepository _decoratedRepository;
    private readonly IAppCache _cache;

    public CachedBookRepositoryImpl(
        IAppCache cache,
        BookRepository decoratedRepository)
    {
        _cache = cache;
        _decoratedRepository = decoratedRepository;
    }

    public async Task<bool> DeleteByIdAsync(int id) => await _decoratedRepository.DeleteByIdAsync(id);

    public async Task<int> CountAsync() => await _decoratedRepository.CountAsync();

    public async ValueTask<int> CountAsync(ISpecification<Book> spec) => await _decoratedRepository.CountAsync(spec);

    //TODO: Consider to cache the result of this method
    public async Task<Book?> GetByIdAsync(ISpecification<Book> spec) => await _decoratedRepository.GetByIdAsync(spec);

    public async Task<List<Review>> GetBookReviews(int bookId)
    {
        string cacheKey = $"{nameof(Book)}-{bookId}-Reviews";

        var result = await _cache.GetOrAddAsync(
            cacheKey,
            async () => await _decoratedRepository.GetBookReviews(bookId),
            DateTimeOffset.Now.AddMinutes(5));

        return result;
    }

    public async Task<List<Author>> GetAuthors(AuthorListByIdQuerySpec authorListByIdQuerySpec) =>
        await _decoratedRepository.GetAuthors(authorListByIdQuerySpec);

    public async Task<List<Book>> GetAllListAsync(ISpecification<Book> spec) =>
        await _decoratedRepository.GetAllListAsync(spec);

    public async Task<List<Book>> GetAllListAsync() => await _decoratedRepository.GetAllListAsync();

    public IQueryable<Book> GetAll() => _decoratedRepository.GetAll();

    public async Task<Book?> GetByIdAsync(int id)
    {
        string cacheKey = $"{nameof(Book)}-{id}";
        var result = await _cache.GetOrAddAsync(
            cacheKey,
            async () => await _decoratedRepository.GetByIdAsync(id),
            DateTimeOffset.Now.AddMinutes(5));

        return result;
    }

    public async Task<bool> InsertAsync(Book entity) => await _decoratedRepository.InsertAsync(entity);
}