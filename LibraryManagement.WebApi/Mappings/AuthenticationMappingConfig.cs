using LibraryManagement.Application.Contracts.Authentications;
using LibraryManagement.WebApi.Models;
using Mapster;

namespace LibraryManagement.WebApi.Mappings;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<string, ValidateTokenQuery>()
            .Map(dest => dest.ValidateToken, src => src);

        config.NewConfig<LoginRequest, LoginQuery>()
            .Map(dest => dest, src => src);

        config.NewConfig<RegisterRequest, RegisterCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<(ChangePasswordRequest,string), ChangePasswordCommand>()
            .Map(dest => dest.Id, src => src.Item2)
            .Map(dest => dest.CurrentPassword, src => src.Item1.CurrentPassword)
            .Map(dest => dest.NewPassword, src => src.Item1.NewPassword);

        config.NewConfig<ResetPasswordRequest, ResetPasswordCommand>();
    }
}