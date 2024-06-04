using System.Security.Claims;
using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.Application.Contracts.Books.ReviewDtos;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Application.Contracts.Users;
using LibraryManagement.Domain.Shared.Params;
using LibraryManagement.WebApi.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebApi.Controllers;

public class BookController : ApiController
{
    private readonly IBookServices _bookServices;
    private readonly IWishListServices _wishListServices;

    public BookController(
        IMapper mapper,
        ILogger<BookController> logger,
        IBookServices bookServices,
        IWishListServices wishListServices)
        : base(mapper, logger)
    {
        _bookServices = bookServices;
        _wishListServices = wishListServices;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAllBooks([FromQuery] BookFilterParams filterParams)
    {
        var result = await _bookServices.GetBooksAsync(filterParams);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var result = await _bookServices.GetBookByIdAsync(id);

        return Ok(result);
    }

    [Authorize("RequireAdministratorRole")]
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> UpsertBook([FromBody] BookForUpsertDto bookForUpsertDto)
    {
        var result = await _bookServices.UpsertBookAsync(bookForUpsertDto);

        return Ok(result);
    }

    [Authorize("RequireAdministratorRole")]
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var result = await _bookServices.DeleteBookAsync(id);

        return Ok(result);
    }

    [HttpGet]
    [Route("{bookId}/reviews")]
    public async Task<IActionResult> GetReviews(int bookId)
    {
        var result = await _bookServices.GetReviews(bookId);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("{bookId}/add-review")]
    public async Task<IActionResult> AddReview(int bookId, [FromBody] AddReviewRequest reviewRequest)
    {
        string? userId = User.FindFirstValue(ClaimTypes.Name);

        if (userId is null)
        {
            return Unauthorized();
        }

        var reviewForCreateDto = new ReviewForCreateDto(
            reviewRequest.Title,
            reviewRequest.Content,
            reviewRequest.ImageUrl,
            bookId,
            new Guid(userId),
            reviewRequest.IsLike,
            reviewRequest.Rating);

        var result = await _bookServices.AddReviewAsync(reviewForCreateDto);

        return Ok(result);
    }

    [HttpPut]
    [Route("{bookId}/add-to-wish-list")]
    public async Task<IActionResult> AddBookToWishList(int bookId)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var addBookToWishListRequest = new WishlistItemForAddRemoveDto(new Guid(userId), bookId);

        var user = await _wishListServices.AddItemToWishList(addBookToWishListRequest);
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user);
    }

    [HttpPut]
    [Route("{bookId}/remove-from-wish-list")]
    public async Task<IActionResult> RemoveBookFromWishList(int bookId)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var addBookToWishListRequest = new WishlistItemForAddRemoveDto(new Guid(userId), bookId);

        var user = await _wishListServices.RemoveItemFromWishList(addBookToWishListRequest);
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user);
    }

    [HttpPut]
    [Route("{bookId}/add-favourite-book")]
    public async Task<IActionResult> AddFavouriteBook(int bookId)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var addBookToWishListRequest = new WishlistItemForAddRemoveDto(new Guid(userId), bookId);

        var user = await _wishListServices.AddFavouriteBook(addBookToWishListRequest);
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user);
    }

    [HttpPut]
    [Route("{bookId}/remove-favourite-book")]
    public async Task<IActionResult> RemoveFavouriteBook(int bookId)
    {
        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        if (userId is null)
        {
            return Unauthorized();
        }

        var addBookToWishListRequest = new WishlistItemForAddRemoveDto(new Guid(userId), bookId);

        var user = await _wishListServices.RemoveFavouriteBook(addBookToWishListRequest);
        return (!user.IsSuccess) ? BadRequest(user) : Ok(user);
    }
}