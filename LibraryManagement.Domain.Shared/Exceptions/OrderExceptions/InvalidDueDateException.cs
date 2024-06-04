namespace LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

public class InvalidDueDateException : Exception
{
    public override string Message { get; } = "Due date must be greater than borrow date";
}