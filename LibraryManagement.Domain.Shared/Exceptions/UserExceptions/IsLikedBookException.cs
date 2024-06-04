namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class IsLikedBookException : Exception
{
    public override string Message { get; } = "This book is already liked by this customer";
}