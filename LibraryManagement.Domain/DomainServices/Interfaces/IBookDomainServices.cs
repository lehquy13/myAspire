using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Domain.DomainServices.Interfaces;

public interface IBookDomainServices
{
    Task<Result> AddFavouriteBook(IdentityGuid customerId, int bookId);
    Task<Result> RemoveFavouriteBook(IdentityGuid customerId, int bookId);
    
    Task<Result> AddItemToWishList(IdentityGuid userId, int bookId);
    Task<Result> RemoveItemToWishList(IdentityGuid userId, int bookId);
}