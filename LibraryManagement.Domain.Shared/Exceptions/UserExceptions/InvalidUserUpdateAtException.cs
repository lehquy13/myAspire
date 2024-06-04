namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class InvalidUserUpdateAtException : Exception
{
    public override string Message { get; } = "Invalid user update at! User update at must be greater than CreateAt.";
}