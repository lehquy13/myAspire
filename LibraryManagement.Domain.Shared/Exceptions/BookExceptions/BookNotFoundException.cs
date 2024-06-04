namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class BookNotFoundException : Exception
{
    public override string Message { get; } = "Book was not found";
}