namespace LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

public class EmptyOrderException : Exception
{
    public override string Message { get; } = "At least one order item is required";
}