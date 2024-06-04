namespace LibraryManagement.Application.Contracts.Baskets;

public record AddItemToBasketCommand(Guid UserId, int BookId, decimal Price, int Quantity = 1);