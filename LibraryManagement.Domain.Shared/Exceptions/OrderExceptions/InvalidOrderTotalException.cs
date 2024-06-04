namespace LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

public class InvalidOrderTotalException : Exception
{
    public override string Message { get; } = "Invalid order total";
}