namespace LibraryManagement.Domain.Shared.Exceptions.ReviewExceptions;

public class InvalidReviewContentException : Exception
{
    public override string Message { get; } = "Content must have at least 10 characters";
}