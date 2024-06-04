namespace LibraryManagement.Domain.Shared.Exceptions.BasketExceptions;

public class InvalidBasketItemQuantityException : Exception
{
    public override string Message { get; } = "Invalid basket item quantity";
}