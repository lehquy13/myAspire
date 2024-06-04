namespace LibraryManagement.Application.Contracts.Authentications;

public record ResetPasswordCommand
(
    string Email,
    string Otp,
    string NewPassword
);
