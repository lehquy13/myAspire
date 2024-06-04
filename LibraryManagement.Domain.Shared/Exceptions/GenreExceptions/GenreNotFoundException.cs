namespace LibraryManagement.Domain.Shared.Exceptions.GenreExceptions;

public class GenreNotFoundException : Exception
{
    public override string Message { get; } = "Genre was not found";
}