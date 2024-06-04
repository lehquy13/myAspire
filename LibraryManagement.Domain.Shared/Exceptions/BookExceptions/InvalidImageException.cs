namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class InvalidImageException : Exception
{
    public override string Message { get; } = "Invalid image's url. Please check again!";
}