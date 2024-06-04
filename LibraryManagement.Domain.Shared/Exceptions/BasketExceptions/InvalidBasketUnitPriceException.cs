namespace LibraryManagement.Domain.Shared.Exceptions.BasketExceptions;

public class InvalidBasketUnitPriceException : Exception
{
    public override string Message { get; } = "Invalid basket item unit price";
}