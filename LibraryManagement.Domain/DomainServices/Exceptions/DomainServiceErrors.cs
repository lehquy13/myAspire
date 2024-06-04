namespace LibraryManagement.Domain.DomainServices.Exceptions;

public class DomainServiceErrors
{
    public const string AddItemFailWhileSavingChanges = "Add item fail while saving changes";
    public const string CreateOrderFailWhileSavingChanges = "Create order fail while saving changes";
    public const string SetQuantitiesFailWhileSavingChanges = "Set quantities fail while saving changes";
    public const string AddFavoriteBookFailWhileSavingChanges = "Add favorite book fail while saving changes";
    public const string RemoveFavoriteBookFailWhileSavingChanges = "Remove favorite book fail while saving changes";
    
    public const string BasketNotFound = "Basket not found";
    public const string OrderNotFound = "Order not found";
    public const string UserNotFound = "User not found";
    public const string PurchaseOrderFailWhileSavingChanges = "Purchase order fail while saving changes";
    public const string InvalidPassword = "Invalid password";
    public const string ChangePasswordFailWhileSavingChanges = "Change password fail while saving changes";
    public const string CreateUserFailWhileSavingChanges = "Create user fail while saving changes";
    public const string RoleNotFound = "Role not found";
    public const string BookNotFound = "Book not found";
    public const string BookNotFoundInWishList = "Book not found in wish list";
    public const string OrderAlreadyCompleted = "Order already completed";
    public const string InsufficientBookQuantity = "Insufficient book quantity";
    public const string InsufficientBookQuantityWithSpecificOrderId = "Insufficient book quantity of order with id: ";
    public const string BasketIsEmpty = "Basket is empty";
    public const string InsufficientUserBalance = "Insufficient user balance";
    public const string InsertUserFail = "Insert user fail at IdentityDomainServices.cs";
    public const string InsertUserBasketFail = "Insert user's basket fail at IdentityDomainServices.cs";
    public const string BasketItemNotFound = "Basket item not found at BasketDomainServices.cs";
}