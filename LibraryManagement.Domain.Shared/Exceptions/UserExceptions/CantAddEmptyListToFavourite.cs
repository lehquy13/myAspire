namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class CantAddEmptyListToFavourite : Exception
{
    public override string Message { get; } = $"Cant add an empty list to user's favourite books";
}

public class CantAddEmptyListToWistlist : Exception
{
    public override string Message { get; } = $"Cant add an empty list to user's wishlist";
}