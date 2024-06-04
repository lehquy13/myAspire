namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class InvalidUserAddressException : Exception
{
    public override string Message { get; } = "Invalid user's format format";
}