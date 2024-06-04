namespace LibraryManagement.Application.Contracts.Commons.ErrorMessages;

public class OrderErrorMessages
{
    public const string PurchaseOrderFailWhileSavingChanges = "Purchase order fails while saving changes";
    public const string OrderNotFound = "Order not found";
    public const string CreateOrderFailWhileSavingChanges = "Create order fails while saving changes";
    public const string CreateOrderFail = "Create order fails at OrderServices.cs";
    public const string CreateOrderFailWithException = "Create order fails at OrderServices.cs with exception: ";
}