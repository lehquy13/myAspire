namespace LibraryManagement.Domain.Shared.Exceptions.AuthorExceptions;

public class AuthorNotFoundException : Exception
{
    public override string Message { get; } = "Author was not found";
}