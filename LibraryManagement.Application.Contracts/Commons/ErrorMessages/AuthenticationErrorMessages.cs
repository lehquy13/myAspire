namespace LibraryManagement.Application.Contracts.Commons.ErrorMessages;

public class AuthenticationErrorMessages
{
    public const string EmailAlreadyExists = "Email already exists";
    public const string EmailNotFound = "Email not found";
    public const string InvalidPassword = "Invalid password";
    public const string LoginFail = "Login fail";
    public const string ChangePasswordFailWhileSavingChanges = "Change password fail while saving changes";
    public const string InvalidToken = "Invalid token";
    public const string RegisterFail = "Register fail";
    public const string ResetPasswordFail = "Reset password fail";
    public const string InvalidOtp = "Invalid otp";
    public const string ResetPasswordFailWhileSavingChanges = "Reset password fail while saving changes";
    public const string ChangePasswordFail = "Change password fail";
}

//class for success login, register
public class AuthenticationSuccessMessages
{
    public const string LoginSuccess = "User has login!";
    public const string RegisterSuccess = "New user has registerd!";


}