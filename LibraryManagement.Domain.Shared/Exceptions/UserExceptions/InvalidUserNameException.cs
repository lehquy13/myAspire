namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class InvalidUserNameException : Exception
{
    public override string Message { get; } = "Invalid user's name format";
}