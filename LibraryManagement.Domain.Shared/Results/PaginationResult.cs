using LibraryManagement.Domain.Shared.Paginations;

namespace LibraryManagement.Domain.Shared.Results;

public class PaginationResult<T> : ResultBase, IPaginated, IHasTotalItemsCount, IHasTotalPagesCount
    where T : notnull
{
    public int PageIndex { get; private set; }
    
    public int PageSize { get; private set; }

    public int TotalItems { get; private set; }

    public int TotalPages { get; private set; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public List<T> Value { get; private set; } = default!;

    private PaginationResult()
    {
    }

    private PaginationResult(List<T> value, int totalCount, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalItems = totalCount;
        PageSize = pageSize;
        Value = value;
    }

    public static PaginationResult<T> Success(
        List<T> items,
        int totalCount,
        int pageIndex,
        int pageSize,
        string message = "")
    {
        return new PaginationResult<T>(items, totalCount, pageIndex, pageSize);
    }
    
    public static PaginationResult<T> Success(
        List<T> items,
        string message = "")
    {
        return new PaginationResult<T>(items, items.Count, 1, items.Count);
    }

    public static PaginationResult<T> Fail(string errorMessage)
    {
        return new PaginationResult<T>
        {
            IsSuccess = false,
            DisplayMessage = errorMessage,
            ErrorMessages = new List<string> { errorMessage }
        };
    }

    public static implicit operator PaginationResult<T>(Result result)
    {
        return new PaginationResult<T>
        {
            IsSuccess = result.IsSuccess,
            DisplayMessage = result.DisplayMessage,
            ErrorMessages = result.ErrorMessages
        };
    }
}