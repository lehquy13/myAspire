namespace LibraryManagement.Application.Contracts.Baskets;

public record SetQuantitiesCommand(Guid UserId, Dictionary<int, int> Quantities);