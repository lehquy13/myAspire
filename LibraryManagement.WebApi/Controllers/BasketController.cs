using System.Security.Claims;
using LibraryManagement.Application.Contracts.Baskets;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.WebApi.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

public class BasketController : AuthorizeApiController
{
    private readonly IBasketServices _basketServices;

    public BasketController(IMapper mapper, ILogger<BasketController> logger, IBasketServices basketServices) : base(
        mapper, logger)
    {
        _basketServices = basketServices;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetBasket()
    {
        string? userId = User.FindFirstValue(ClaimTypes.Name);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _basketServices.GetBasketAsync(new Guid(userId));

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut]
    [Route("add-item-to-basket")]
    public async Task<IActionResult> AddItemToBasket([FromBody] AddItemToBasketRequest addItemToBasketRequest)
    {
        string? userId = User.FindFirstValue(ClaimTypes.Name);

        if (userId is null)
        {
            return Unauthorized();
        }

        var setQuantitiesCommand =
            new AddItemToBasketCommand(new Guid(userId), addItemToBasketRequest.BookId,
                addItemToBasketRequest.Price, addItemToBasketRequest.Quantity);

        var result = await _basketServices.AddItemToBasket(setQuantitiesCommand);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut]
    [Route("update-item-quantities")]
    public async Task<IActionResult> UpdateItemQuantities(
        [FromBody] UpdateItemQuantitiesRequest updateItemQuantitiesRequest)
    {
        string? userId = User.FindFirstValue(ClaimTypes.Name);

        if (userId is null)
        {
            return Unauthorized();
        }

        var bookIdWithQuantity = new Dictionary<int, int>
        {
            { updateItemQuantitiesRequest.BookId, updateItemQuantitiesRequest.Quantity }
        };

        var setQuantitiesCommand =
            new SetQuantitiesCommand(new Guid(userId), bookIdWithQuantity);

        var result = await _basketServices.SetQuantities(setQuantitiesCommand);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut]
    [Route("remove-item/{bookId}")]
    public async Task<IActionResult> RemoveItem(int bookId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.Name);

        if (userId is null)
        {
            return Unauthorized();
        }

        var bookIdWithQuantity = new Dictionary<int, int>
        {
            { bookId, 0 }
        };

        var setQuantitiesCommand =
            new SetQuantitiesCommand(new Guid(userId), bookIdWithQuantity);

        var result = await _basketServices.SetQuantities(setQuantitiesCommand);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}