namespace LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

public class IsAlreadyInWishListException : Exception
{
    public override string Message { get; } = "Book is already in wish list";
}