using LibraryManagement.Application.Contracts.Authentications;

namespace LibraryManagement.Application.Contracts.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserForLoginResponseDto userForLoginResponseDto);
        bool ValidateToken(string token);
    }
}
