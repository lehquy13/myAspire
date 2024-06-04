using LibraryManagement.Application.Contracts.Books;
using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.Application.Contracts.Books.ReviewDtos;
using LibraryManagement.Application.Contracts.Commons.ErrorMessages;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Params;
using LibraryManagement.Domain.Shared.Results;
using LibraryManagement.Domain.Shared.Utilities;
using LibraryManagement.Domain.Specifications.Books;
using LibraryManagement.Domain.Specifications.Orders;
using MapsterMapper;

namespace LibraryManagement.Application.ServiceImpls;

public class BookServices : ServiceBase, IBookServices
{
    private readonly IBookRepository _bookRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;

    public BookServices(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAppLogger<BookServices> logger,
        IBookRepository bookRepository,
        IOrderRepository orderRepository,
        IUserRepository userRepository)
        : base(mapper, unitOfWork, logger)
    {
        _bookRepository = bookRepository;
        _orderRepository = orderRepository;
        _userRepository = userRepository;
    }

    public async Task<PaginationResult<BookForListDto>> GetBooksAsync(BookFilterParams bookFilterParams)
    {
        await Task.CompletedTask;
        var bookListQuerySpec = new BookListQuerySpec(
            bookFilterParams.PageIndex,
            bookFilterParams.PageSize,
            bookFilterParams.Title,
            bookFilterParams.AuthorName,
            bookFilterParams.Genre,
            bookFilterParams.PublicationDate
        );

        int totalCount = await _bookRepository.CountAsync(bookListQuerySpec);

        var books = await _bookRepository.GetAllListAsync(bookListQuerySpec);

        var bookForListDtos = Mapper.Map<List<BookForListDto>>(books);

        return PaginationResult<BookForListDto>
            .Success(
                bookForListDtos,
                totalCount,
                bookFilterParams.PageIndex,
                bookFilterParams.PageSize);
    }

    public async Task<Result<BookForDetailDto>> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            Logger.LogError($"{BookErrorMessages.BookNotFound}",id);
            return Result.Fail(BookErrorMessages.BookNotFound);
        }

        var bookForDetailDto = Mapper.Map<BookForDetailDto>(book);

        return bookForDetailDto;
    }

    public async Task<Result> UpsertBookAsync(BookForUpsertDto bookForUpsertDto)
    {
        var book = await _bookRepository.GetByIdAsync(bookForUpsertDto.Id);

        if (book is null)
        {
            //Create new book
            //book = Mapper.Map<Book>(bookForUpsertDto); 

            var authors = await GetAuthors(bookForUpsertDto.AuthorIds);

            book = Book.Create(
                bookForUpsertDto.Title,
                bookForUpsertDto.Quantity,
                bookForUpsertDto.ImageUrl,
                bookForUpsertDto.Price,
                bookForUpsertDto.CurrentPrice,
                authors,
                bookForUpsertDto.Genre.ToEnum<Genre>(),
                bookForUpsertDto.PublicationDate
            );

            await _bookRepository.InsertAsync(book);
        }
        else 
        {
            var authors = await GetAuthors(bookForUpsertDto.AuthorIds);
            Mapper.Map(bookForUpsertDto, book);

            book.Authors = authors;
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(BookErrorMessages.UpsertFailWhileSavingChanges);
        }

        return Result.Success();
    }

    private async Task<List<Author>> GetAuthors(List<int> argues)
    {
        var authors = await _bookRepository.GetAuthors(
            new AuthorListByIdQuerySpec(argues)
        );
        return authors;
    }

    public async Task<Result> DeleteBookAsync(int id)
    {
        var deleteResult = await _bookRepository.DeleteByIdAsync(id);

        if (deleteResult is false)
        {
            Logger.LogError($"{BookErrorMessages.BookNotFound} with Id: {id}", id);
            return Result.Fail(BookErrorMessages.BookNotFound);
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(BookErrorMessages.DeleteFailWhileSavingChanges);
        }
        Logger.LogInformation($"Book with Id: {id} has been deleted", id);

        return Result.Success();
    }

    /// <summary>
    /// Mark not to use, bc GetBookByIdAsync already has this function
    /// </summary>
    /// <param name="bookId"></param>
    /// <returns></returns>
    public async Task<Result<List<ReviewForListDto>>> GetReviews(int bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);

        if (book is null)
        {
            Logger.LogError($"{BookErrorMessages.BookNotFound} with Id: {bookId}",  bookId);
            return Result.Fail(BookErrorMessages.BookNotFound);
        }

        var reviews = book.Reviews;

        var reviewForListDtos = Mapper.Map<List<ReviewForListDto>>(reviews);

        return reviewForListDtos;
    }

    public async Task<Result> AddReviewAsync(ReviewForCreateDto reviewForCreateDto)
    {
        var book = await _bookRepository.GetByIdAsync(reviewForCreateDto.BookId);

        if (book is null)
        {
            Logger.LogError($"{BookErrorMessages.BookNotFound} with Id: {reviewForCreateDto.BookId}",reviewForCreateDto.CustomerId);
            return Result.Fail(BookErrorMessages.BookNotFound);
        }

        if (book.Reviews.Select(x => x.UserId.Value).Any(id => id == reviewForCreateDto.CustomerId))
        {
            return Result.Fail(BookErrorMessages.UserHasAlreadyReviewedThisBook);
        }

        var cus = await _userRepository.GetByIdAsync(IdentityGuid.Create(reviewForCreateDto.CustomerId));

        if (cus is null)
        {
            Logger.LogError($"{UserErrorMessages.UserNotFound} with Id: {reviewForCreateDto.BookId}",reviewForCreateDto.CustomerId);
            return Result.Fail(UserErrorMessages.UserNotFound);
        }

        //Check have customer bought the book yet
        var orders = await _orderRepository.GetAllListAsync(
            new OrderHavingBookByIdSpec(reviewForCreateDto.BookId, IdentityGuid.Create(reviewForCreateDto.CustomerId))
        );

        if (orders.Count <= 0)
        {
            return Result.Fail(BookErrorMessages.UserMustBuyBookBeforeReviewing);
        }

        var review = Mapper.Map<Review>(reviewForCreateDto);

        book.AddReview(review);

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(BookErrorMessages.UpsertFailWhileSavingChanges);
        }
        Logger.LogInformation("Success review book!",  reviewForCreateDto.CustomerId);

        return Result.Success();
    }
}