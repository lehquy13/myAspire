namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class InvalidUserBalanceAmountException : Exception
{
    public override string Message { get; } = "Invalid user's balance amount. It must be greater than or equal to 0.";
}