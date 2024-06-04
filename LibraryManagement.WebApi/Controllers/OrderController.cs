using System.Security.Claims;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Application.Contracts.Order;
using LibraryManagement.Domain.Shared.Paginations;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

public class OrderController : AuthorizeApiController
{
    private readonly IOrderServices _orderServices;

    public OrderController(IMapper mapper, ILogger<OrderController> logger, IOrderServices orderServices) : base(mapper,
        logger)
    {
        _orderServices = orderServices;
    }

    [Authorize("RequireAdministratorRole")]
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllOrders([FromQuery] OrderPaginatedParams orderPaginatedParams)
    {
        var result = await _orderServices.GetAllOrders(orderPaginatedParams);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [Authorize("RequireAdministratorRole")]
    [HttpGet]
    [Route("{orderId}")]
    public async Task<IActionResult> GetOrder(string orderId)
    {
        var result = await _orderServices.GetOrder(new Guid(orderId));

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("get-my-orders")]
    public async Task<IActionResult> GetMyOrders([FromQuery] PaginatedParams paginatedParams)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        OrderPaginatedParams orderPaginatedParams = new()
        {
            PageIndex = paginatedParams.PageIndex,
            PageSize = paginatedParams.PageSize,
            UserId = new Guid(userId)
        };

        var result = await _orderServices.GetAllOrders(orderPaginatedParams);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost]
    [Route("create-order")]
    public async Task<IActionResult> CreateOrder()
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _orderServices.CreateOrder(new Guid(userId));

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut]
    [Route("{orderId}/purchase")]
    public async Task<IActionResult> PurchaseOrder(string orderId, string paymentMethod)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _orderServices.PurchaseOrder(Guid.Parse(userId), Guid.Parse(orderId), paymentMethod);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}