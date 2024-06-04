using System.Security.Claims;
using LibraryManagement.Application.Contracts.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

public class ProfileController : AuthorizeApiController
{
    private readonly IUserServices _userServices;
    private readonly IWishListServices _wishListServices;

    public ProfileController(
        IMapper mapper,
        ILogger<ProfileController> logger, 
        IUserServices userServices, IWishListServices wishListServices) : base(mapper, logger)
    {
        _userServices = userServices;
        _wishListServices = wishListServices;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetProfileDetail()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var user = await _userServices.GetUserDetailByIdAsync(new Guid(userId));
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user);
    }
    
    [HttpGet]
    [Route("wish-list")]
    public async Task<IActionResult> GetUserWishList()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var user = await _userServices.GetUserBasicByIdAsync(new Guid(userId));
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user.Value.WishList);
    }

    [HttpGet]
    [Route("favourite-books")]
    public async Task<IActionResult> GetUserFavouriteBook()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var user = await _userServices.GetUserBasicByIdAsync(new Guid(userId));
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user.Value.FavouriteBooks);
    }

    
}