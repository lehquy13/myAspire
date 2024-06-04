namespace LibraryManagement.Domain.Shared.Paginations;

public class PaginatedParams
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
}