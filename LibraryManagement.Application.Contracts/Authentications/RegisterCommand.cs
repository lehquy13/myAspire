namespace LibraryManagement.Application.Contracts.Authentications;

public record RegisterCommand
(
    string Name,
    string Email,
    string Password,
    string City,
    string Country,
    string PhoneNumber
);
