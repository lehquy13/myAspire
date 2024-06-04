namespace LibraryManagement.Domain.Shared.Exceptions.GenreExceptions;

public class InvalidGenreNameException : Exception
{
    public override string Message { get; } = "Invalid genre's name format";
}