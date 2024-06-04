namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class InvalidUserPhoneNumberException : Exception
{
    public override string Message { get; } = "Invalid user's phone number format.";
}