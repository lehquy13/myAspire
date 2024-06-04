namespace LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

public class OrderNotFoundException : Exception
{
    public override string Message { get; } = "Order not found";

    public const string Code = "OrderNotFound";
}