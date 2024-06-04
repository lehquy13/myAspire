using System.Security.Claims;
using LibraryManagement.Application.Contracts.Authentications;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.WebApi.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

public class AuthenticationController : ApiController
{
    private readonly IAuthenticationServices _authenticationServices;

    public AuthenticationController(ILogger<AuthenticationController> logger, IMapper mapper,
        IAuthenticationServices authenticationServices) : base(mapper, logger)
    {
        _authenticationServices = authenticationServices;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginQuery loginRequest)
    {
        var result = await _authenticationServices.Login(loginRequest);
        //throw new Exception("CMM");
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterCommand registerRequest)
    {
        var result = await _authenticationServices.Register(registerRequest);

        if (!result.IsSuccess)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var result = await _authenticationServices.ForgotPassword(email);

        if (!result.IsSuccess)
        {
            return BadRequest(result.DisplayMessage);
        }

        return NoContent();
    }

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand resetPasswordRequest)
    {
        var result = await _authenticationServices.ResetPassword(resetPasswordRequest);

        if (!result.IsSuccess)
        {
            return BadRequest(result.DisplayMessage);
        }

        return NoContent();
    }

    [Authorize]
    [HttpPost]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        string? userId = User.FindFirstValue(ClaimTypes.Name);
        if (userId is null)
        {
            return Unauthorized();
        }

        var changePasswordCommand = Mapper.Map<ChangePasswordCommand>((changePasswordRequest, userId));

        var result = await _authenticationServices.ChangePassword(changePasswordCommand);

        if (!result.IsSuccess)
        {
            return BadRequest(result.DisplayMessage);
        }

        return NoContent();
    }
}