namespace LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

public class InvalidPaymentAmountException : Exception
{
    public override string Message => "Payment amount must be >= 0";
}