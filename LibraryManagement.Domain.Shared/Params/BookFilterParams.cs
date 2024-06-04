using LibraryManagement.Domain.Shared.Paginations;

namespace LibraryManagement.Domain.Shared.Params;

public sealed class BookFilterParams : PaginatedParams
{
    public string? Title { get; init; } = string.Empty;
    public string? AuthorName { get; init; } = string.Empty;
    public DateTime PublicationDate { get; init; } = new DateTime();
    public string? Genre { get; init; } = string.Empty;
}