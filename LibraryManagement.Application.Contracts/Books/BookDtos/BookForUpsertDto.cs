using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Books.BookDtos;

public class BookForUpsertDto : EntityDto<int>
{
    public decimal Price { get; set; }
    public decimal CurrentPrice { get; set; }
    public string Title { get; set; } = string.Empty;

    public List<int> AuthorIds { get; set; } = new();

    public string Genre { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateTime PublicationDate { get; init; } = new DateTime();

    public string ImageUrl { get; set; } = string.Empty;
}