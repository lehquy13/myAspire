namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class InvalidBookTitleException : Exception
{
    public override string Message { get; } = "Invalid book title format";
}