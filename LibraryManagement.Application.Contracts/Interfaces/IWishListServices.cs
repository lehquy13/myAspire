using LibraryManagement.Application.Contracts.Users;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Application.Contracts.Interfaces;

public interface IWishListServices
{
    Task<Result> AddItemToWishList(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto);
    Task<Result> RemoveItemFromWishList(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto);
    
    Task<Result> AddFavouriteBook(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto);
    Task<Result> RemoveFavouriteBook(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto);
}