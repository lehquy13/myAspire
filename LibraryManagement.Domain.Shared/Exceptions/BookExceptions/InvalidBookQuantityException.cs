namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class InvalidBookQuantityException : Exception
{
    public override string Message { get; } = "Invalid book quantity format! It must be greater than 0";
}