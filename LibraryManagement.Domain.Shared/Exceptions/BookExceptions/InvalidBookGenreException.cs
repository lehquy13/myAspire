namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class InvalidBookPriceException : Exception
{
    public override string Message { get; } = "Invalid book price. Price must be greater than 0.";
}