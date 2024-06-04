namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class InvalidBookPublishYearException : Exception
{
    public override string Message { get; } = "Invalid book publishDate year format";
}