namespace LibraryManagement.Application.Contracts.Authentications;

public record ChangePasswordCommand
(
    string Id,
    string CurrentPassword,
    string NewPassword
);