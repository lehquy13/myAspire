namespace LibraryManagement.Application.Contracts.Commons.ErrorMessages;

public static class UserErrorMessages
{
    public const string UserNotFound = "User not found";
    
    public const string UserAlreadyExist = "User already exist";

    public const string UpdateUserFailWhenSavingChanges = "Update User fail when saving changes at UserServices.cs";
    public const string CreateUserFailWhenSavingChanges = "Create User fail when saving changes at UserServices.cs";
    public const string DeleteUserFailWhenSavingChanges = "Delete User fail when saving changes at UserServices.cs";
    public const string UpsertFailWithException = "Upsert User fail with exception: ";
    public const string DeleteFailWithException = "Delete User fail with exception: ";
    public const string AddItemToWishListFailWhileSavingChanges = "Add item to wish list fail while saving changes at WishlistServices.cs";
    public const string AddItemToWishListFail = "Add item to wish list fail at WishlistServices.cs";
    public const string AddFavoriteBookFailWhileSavingChanges = "Add favorite book fail while saving changes at WishlistServices.cs";
    public const string RemoveFavouriteBookFailWhileSavingChanges = "Remove favourite book fail while saving changes at WishlistServices.cs";
    public const string DepositFailWhileSavingChanges = "Deposit fail while saving changes";
    public const string RemoveItemFromWishListFail = "Remove item from wish list fail at WishlistServices.cs";
    public const string RemoveItemFromWishListFailWhileSavingChanges = "Remove item from wish list fail while saving changes at WishlistServices.cs";
    public const string CreateUserFailWhileSavingChanges = "Create User fail while saving changes at UserServices.cs";
}