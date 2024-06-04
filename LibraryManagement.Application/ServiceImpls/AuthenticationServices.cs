using LibraryManagement.Application.Contracts.Authentications;
using LibraryManagement.Application.Contracts.Commons.ErrorMessages;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;
using MapsterMapper;

namespace LibraryManagement.Application.ServiceImpls;

public class AuthenticationServices : ServiceBase, IAuthenticationServices
{
    private readonly IIdentityDomainServices _identityDomainServices;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmailSender _emailSender;

    public AuthenticationServices(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IAppLogger<ServiceBase> logger,
        IIdentityDomainServices identityDomainServices,
        IJwtTokenGenerator jwtTokenGenerator,
        IEmailSender emailSender)
        : base(mapper, unitOfWork, logger)
    {
        _identityDomainServices = identityDomainServices;
        _jwtTokenGenerator = jwtTokenGenerator;
        _emailSender = emailSender;
    }

    public async Task<Result<AuthenticationResult>> Login(LoginQuery loginQuery)
    {
        var identityUser = await _identityDomainServices.SignInAsync(loginQuery.Email, loginQuery.Password);

        if (identityUser is null)
        {
            return Result.Fail(AuthenticationErrorMessages.LoginFail);
        }

        var userLoginDto = Mapper.Map<UserForLoginResponseDto>(identityUser);

        var token = _jwtTokenGenerator.GenerateToken(userLoginDto);

        return new AuthenticationResult(userLoginDto, token);
    }

    public async Task<Result> ResetPassword(ResetPasswordCommand changePasswordCommand)
    {
        var identityUser = await _identityDomainServices.FindByEmailAsync(changePasswordCommand.Email);

        if (identityUser is null)
        {
            return Result.Fail(AuthenticationErrorMessages.LoginFail);
        }

        //Check does otp have same value and still valid
        if (identityUser.OtpCode.Value != changePasswordCommand.Otp || identityUser.OtpCode.IsExpired())
        {
            return Result.Fail(AuthenticationErrorMessages.InvalidOtp);
        }

        identityUser.ChangePassword(changePasswordCommand.NewPassword);
        identityUser.OtpCode.Reset();

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(AuthenticationErrorMessages.ResetPasswordFailWhileSavingChanges);
        }

        return Result.Success();
    }

    public async Task<Result> ChangePassword(ChangePasswordCommand changePasswordCommand)
    {
        var identityId = IdentityGuid.Create(new Guid(changePasswordCommand.Id));
        var result = await _identityDomainServices
            .ChangePassword(
                identityId,
                changePasswordCommand.CurrentPassword,
                changePasswordCommand.NewPassword);

        if (!result.IsSuccess)
        {
            var resultToReturn = Result
                .Fail(AuthenticationErrorMessages.ChangePasswordFail)
                .WithErrors(result.ErrorMessages);
            return resultToReturn;
        }
        
        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(AuthenticationErrorMessages.ChangePasswordFailWhileSavingChanges);
        }
        
        return Result.Success();
    }

    public async Task<Result> ForgotPassword(string email)
    {
        var identityUser = await _identityDomainServices.FindByEmailAsync(email);

        if (identityUser is null)
        {
            return Result.Fail(AuthenticationErrorMessages.LoginFail);
        }

        identityUser.GenerateOtpCode();

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(AuthenticationErrorMessages.ResetPasswordFail);
        }

        var message = $"Your OTP code is {identityUser.OtpCode.Value}";

#pragma warning disable
        _emailSender.SendEmail(email, "Reset password", message);
#pragma warning restore

        return Result.Success();
    }

    public async Task<Result<AuthenticationResult>> Register(RegisterCommand registerCommand)
    {
        try
        {
            var result = await _identityDomainServices.CreateAsync(
                registerCommand.Email,
                registerCommand.Password,
                registerCommand.PhoneNumber,
                registerCommand.Name,
                registerCommand.City,
                registerCommand.Country
            );

            if (!result.IsSuccess)
            {
                var resultToReturn = Result.Fail(AuthenticationErrorMessages.RegisterFail);
                resultToReturn.ErrorMessages.Add(result.DisplayMessage);
                return resultToReturn;
            }

            if (await UnitOfWork.SaveChangesAsync() < 0)
            {
                return Result.Fail(UserErrorMessages.CreateUserFailWhileSavingChanges);
            }

            var userLoginDto = Mapper.Map<UserForLoginResponseDto>(result.Value);

            var token = _jwtTokenGenerator.GenerateToken(userLoginDto);

            return new AuthenticationResult(userLoginDto, token);
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);

            var resultToReturn = Result.Fail(AuthenticationErrorMessages.RegisterFail);
            resultToReturn.ErrorMessages.AddRange(resultToReturn.ErrorMessages);
            resultToReturn.ErrorMessages.Add(e.Message);

            return resultToReturn;
        }
    }

    public async Task<Result> ValidateToken(ValidateTokenQuery validateTokenQuery)
    {
        await Task.CompletedTask;
        if (_jwtTokenGenerator.ValidateToken(validateTokenQuery.ValidateToken))
        {
            return Result.Success();
        }

        return Result.Fail(AuthenticationErrorMessages.InvalidToken);
    }
}