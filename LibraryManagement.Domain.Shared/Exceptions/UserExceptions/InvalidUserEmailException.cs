namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class InvalidUserEmailException : Exception
{
    public override string Message { get; } = "Invalid user's mail format";
}