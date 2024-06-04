namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class UserNotFoundException : Exception
{
    public override string Message { get; } = "User was not found";
}