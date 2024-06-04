using LibraryManagement.Domain.DomainServices.Exceptions;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Domain.DomainServices;

public class BookDomainServices : IBookDomainServices
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAppLogger<BookDomainServices> _logger;

    public BookDomainServices(
        IBookRepository bookRepository,
        IUserRepository userRepository,
        IAppLogger<BookDomainServices> logger
    )
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Result> AddFavouriteBook(IdentityGuid customerId, int bookId)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book is null)
            {
                _logger.LogError(DomainServiceErrors.BasketNotFound);
                return Result.Fail(DomainServiceErrors.BasketNotFound);
            }

            var customer = await _userRepository.GetFullById(customerId); // 

            if (customer is null)
            {
                _logger.LogError(DomainServiceErrors.UserNotFound);
                return Result.Fail(DomainServiceErrors.UserNotFound);
            }

            customer.AddFavoriteBook(
                new()
                {
                    book
                }
            );

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> RemoveFavouriteBook(IdentityGuid customerId, int bookId)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book is null)
            {
                _logger.LogError(DomainServiceErrors.BasketNotFound);
                return Result.Fail(DomainServiceErrors.BasketNotFound);
            }

            var customer = await _userRepository.GetFullById(customerId);

            if (customer is null)
            {
                _logger.LogError(DomainServiceErrors.UserNotFound);
                return Result.Fail(DomainServiceErrors.UserNotFound);
            }

            customer.RemoveFavoriteBook(
                new()
                {
                    book
                }
            );

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> AddItemToWishList(IdentityGuid userId, int bookId)
    {
        try
        {
            var user = await _userRepository.GetFullById(userId);

            if (user is null)
            {
                _logger.LogError(DomainServiceErrors.UserNotFound);
                return Result.Fail(DomainServiceErrors.UserNotFound);
            }

            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book is null)
            {
                _logger.LogError(DomainServiceErrors.BookNotFound);
                return Result.Fail(DomainServiceErrors.BookNotFound);
            }

            user.AddToWishList(
                new()
                {
                    book
                }
            );

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> RemoveItemToWishList(IdentityGuid userId, int bookId)
    {
        try
        {
            var user = await _userRepository.GetFullById(userId);

            if (user is null)
            {
                _logger.LogError(DomainServiceErrors.UserNotFound);
                return Result.Fail(DomainServiceErrors.UserNotFound);
            }

            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book is null)
            {
                _logger.LogError(DomainServiceErrors.BookNotFound);
                return Result.Fail(DomainServiceErrors.BookNotFound);
            }

            if (!user.WishLists.Contains(book))
            {
                return Result.Fail(DomainServiceErrors.BookNotFoundInWishList);
            }

            user.RemoveFromWishList(
                new()
                {
                    book
                }
            );

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Result.Fail(e.Message);
        }
    }
}