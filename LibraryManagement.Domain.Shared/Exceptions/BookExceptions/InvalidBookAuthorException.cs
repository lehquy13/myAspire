namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class InvalidBookAuthorException : Exception
{
    public override string Message { get; } = "Book must have at least one author.";
}