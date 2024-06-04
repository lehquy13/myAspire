namespace LibraryManagement.Domain.Shared.Paginations;

public class PaginatedList<T> : List<T>, IPaginated, IHasTotalItemsCount, IHasTotalPagesCount
{
    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }

    public int TotalItems { get; private set; }

    public int TotalPages { get; private set; }

    private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        AddRange(items);
    }

    private PaginatedList()
    {
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize, int count = 0)
    {
        var enumerable = source as T[] ?? source.ToArray();
        if (count == 0)
            count = enumerable.Count();
        //var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(enumerable.ToList(), count, pageIndex, pageSize);
    }
}