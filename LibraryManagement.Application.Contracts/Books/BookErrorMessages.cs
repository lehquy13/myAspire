namespace LibraryManagement.Application.Contracts.Books;

public class BookErrorMessages
{
    public const string BookNotFound = "Book not found";
    public const string AuthorNotFound = "Author not found";
    
    public const string UpsertFailWhileSavingChanges = "Upsert fail while saving changes";
    public const string DeleteFailWhileSavingChanges = "Upsert fail while saving changes";
    public const string UserHasAlreadyReviewedThisBook = "User has already reviewed this book";
    public const string UserMustBuyBookBeforeReviewing = "User must buy book before reviewing";
}