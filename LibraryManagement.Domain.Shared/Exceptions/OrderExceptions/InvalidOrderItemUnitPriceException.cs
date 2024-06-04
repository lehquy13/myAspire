namespace LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

public class InvalidOrderItemUnitPriceException : Exception
{
    public override string Message { get; } = "Unit price must be greater than 0";
}