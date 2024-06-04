namespace LibraryManagement.Application.Contracts.Authentications;

public record AuthenticationResult(UserForLoginResponseDto UserForLoginResponseDto, string Token);
