namespace LibraryManagement.Domain.Shared.Exceptions.ReviewExceptions;

public class InvalidReviewTitleException : Exception
{
    public override string Message { get; } = "Invalid review title format";
}