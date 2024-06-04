using LibraryManagement.Application.Contracts.Authentications;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Application.Contracts.Interfaces;

public interface IAuthenticationServices
{
    Task<Result<AuthenticationResult>> Login(LoginQuery loginQuery);
    Task<Result> ChangePassword(ChangePasswordCommand changePasswordCommand);
    Task<Result> ForgotPassword(string email);
    Task<Result<AuthenticationResult>> Register(RegisterCommand registerCommand);

    Task<Result> ValidateToken(ValidateTokenQuery validateTokenQuery);
    Task<Result> ResetPassword(ResetPasswordCommand resetPasswordCommand);
}