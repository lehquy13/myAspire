namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class InsufficientBalanceAmountException : Exception
{
    public override string Message { get; } = "Insufficient balance amount.";
}