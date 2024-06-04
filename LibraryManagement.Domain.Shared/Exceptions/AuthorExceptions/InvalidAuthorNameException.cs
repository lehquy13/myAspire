namespace LibraryManagement.Domain.Shared.Exceptions.AuthorExceptions;

public class InvalidAuthorNameException : Exception
{
    public override string Message { get; } = "Invalid author's name format";
}