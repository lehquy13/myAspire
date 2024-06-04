namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class InvalidReviewRatingException : Exception
{
    public override string Message { get; } = "Rating must be between 1 and 5";
}