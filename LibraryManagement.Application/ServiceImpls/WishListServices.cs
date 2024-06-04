using LibraryManagement.Application.Contracts.Commons.ErrorMessages;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Application.Contracts.Users;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;
using MapsterMapper;

namespace LibraryManagement.Application.ServiceImpls;

public class WishListServices : ServiceBase, IWishListServices
{
    private readonly IBookDomainServices _bookDomainServices;

    public WishListServices(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IAppLogger<ServiceBase> logger,
        IBookDomainServices bookDomainServices)
        : base(mapper, unitOfWork, logger)
    {
        _bookDomainServices = bookDomainServices;
    }

    public async Task<Result> AddItemToWishList(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto)
    {
        var result = await _bookDomainServices
            .AddItemToWishList(
                IdentityGuid.Create(wishlistItemForAddRemoveDto.UserId),
                wishlistItemForAddRemoveDto.BookId);

        if (!result.IsSuccess)
        {
            Logger.LogError(result.DisplayMessage);
            
            return Result
                .Fail(UserErrorMessages.AddItemToWishListFail)
                .WithErrors(result.ErrorMessages);
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            Logger.LogError(UserErrorMessages.AddItemToWishListFailWhileSavingChanges);
            return Result.Fail(UserErrorMessages.AddItemToWishListFailWhileSavingChanges);
        }

        return Result.Success();
    }
    
    /// <summary>
    /// Deprecated method, consider to remove
    /// </summary>
    /// <param name="wishlistItemForAddRemoveDto"></param>
    /// <returns></returns>
    public async Task<Result> AddFavouriteBook(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto)
    {
        var result = await _bookDomainServices
            .AddFavouriteBook(
                IdentityGuid.Create(wishlistItemForAddRemoveDto.UserId),
                wishlistItemForAddRemoveDto.BookId);

        if (!result.IsSuccess)
        {
            return result;
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            Logger.LogError(UserErrorMessages.AddFavoriteBookFailWhileSavingChanges);
            return Result.Fail(UserErrorMessages.AddFavoriteBookFailWhileSavingChanges);
        }

        return Result.Success();
    }

    public async Task<Result> RemoveItemFromWishList(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto)
    {
        var result = await _bookDomainServices.RemoveItemToWishList(
            IdentityGuid.Create(wishlistItemForAddRemoveDto.UserId), wishlistItemForAddRemoveDto.BookId);

        if (!result.IsSuccess)
        {
            Logger.LogError(result.DisplayMessage);
            
            return Result
                .Fail(UserErrorMessages.RemoveItemFromWishListFail)
                .WithErrors(result.ErrorMessages);
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            Logger.LogError(UserErrorMessages.RemoveItemFromWishListFailWhileSavingChanges);
            return Result.Fail(UserErrorMessages.RemoveItemFromWishListFailWhileSavingChanges);
        }

        return Result.Success();
    }

    /// <summary>
    /// Deprecated method, consider to remove
    /// </summary>
    /// <param name="wishlistItemForAddRemoveDto"></param>
    /// <returns></returns>
    public async Task<Result> RemoveFavouriteBook(WishlistItemForAddRemoveDto wishlistItemForAddRemoveDto)
    {
        var result = await _bookDomainServices.RemoveFavouriteBook(
            IdentityGuid.Create(wishlistItemForAddRemoveDto.UserId),
            wishlistItemForAddRemoveDto.BookId);

        if (!result.IsSuccess)
        {
            return result;
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            Logger.LogError(UserErrorMessages.RemoveFavouriteBookFailWhileSavingChanges);
            return Result.Fail(UserErrorMessages.RemoveFavouriteBookFailWhileSavingChanges);
        }

        return Result.Success();
    }
}