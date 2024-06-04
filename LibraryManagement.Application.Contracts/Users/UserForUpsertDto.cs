namespace LibraryManagement.Application.Contracts.Users;

public record UserForUpsertDto(Guid Id, string Name, string City, string Country);