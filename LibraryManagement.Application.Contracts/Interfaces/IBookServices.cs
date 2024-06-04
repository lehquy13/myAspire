using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.Application.Contracts.Books.ReviewDtos;
using LibraryManagement.Domain.Shared.Params;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Application.Contracts.Interfaces;

public interface IBookServices
{
    Task<PaginationResult<BookForListDto>> GetBooksAsync(BookFilterParams bookFilterParams);
    
    Task<Result<BookForDetailDto>> GetBookByIdAsync(int id);
    
    Task<Result> UpsertBookAsync(BookForUpsertDto bookForUpsertDto);
    
    Task<Result> DeleteBookAsync(int id);
    
    /// <summary>
    /// Mark not to use, bc GetBookByIdAsync already has this function
    /// </summary>
    /// <param name="bookId"></param>
    /// <returns></returns>
    Task<Result<List<ReviewForListDto>>> GetReviews(int bookId);
    
    Task<Result> AddReviewAsync(ReviewForCreateDto reviewForCreateDto);
}