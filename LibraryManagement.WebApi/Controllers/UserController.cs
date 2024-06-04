using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Domain.Shared.Paginations;
using LibraryManagement.WebApi.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

[Authorize("RequireAdministratorRole")]
public class UserController : ApiController
{
    private readonly IUserServices _userServices;

    public UserController(
        IMapper mapper,
        ILogger<UserController> logger,
        IUserServices userServices)
        : base(mapper, logger)
    {
        _userServices = userServices;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetUsers([FromQuery] PaginatedParams paginatedParams)
    {
        var users = await _userServices.GetUsers(paginatedParams);

        return (!users.IsSuccess) ? BadRequest(users) : Ok(users);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _userServices.GetUserDetailByIdAsync(new Guid(userId));
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user);
    }

    [HttpPatch]
    [Route("{userId}/deposit")]
    public async Task<IActionResult> Deposit(string userId, [FromBody] DepositRequest deposit)
    {
        var result = await _userServices.DepositAsync(new Guid(userId), deposit.Amount);
        return (!result.IsSuccess) ? BadRequest(result) : Ok(result);
    }
}