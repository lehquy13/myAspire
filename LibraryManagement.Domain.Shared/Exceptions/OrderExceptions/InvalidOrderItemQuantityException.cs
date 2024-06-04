namespace LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

public class InvalidOrderItemQuantityException : Exception
{
    public override string Message { get; } = "Quantity must be greater than 0";
}