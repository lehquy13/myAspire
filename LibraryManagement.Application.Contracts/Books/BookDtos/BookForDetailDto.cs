using LibraryManagement.Application.Contracts.Books.ReviewDtos;
using LibraryManagement.Application.Contracts.Commons.Primitives;
using LibraryManagement.Domain.Shared.Enums;

namespace LibraryManagement.Application.Contracts.Books.BookDtos;

public class BookForDetailDto : EntityDto<int>
{
    public string Title { get; set; } = string.Empty;
    
    public List<AuthorDto> AuthorDtos { get; set; } = null!;

    public Genre Genre { get; set; }

    public int Quantity { get; set; }

    public DateTime PublicationDate { get; set; }
   
    public List<ReviewForListDto> ReviewForListDtos { get; set; } = new();

    public override string ToString()
    {
        return $"Id: {Id}\n" +
               $"Title: {Title}\n" +
               $"Author: {AuthorDtos}\n" +
               $"Genre: {Genre}\n" +
               $"Quantity: {Quantity}\n" +
               $"PublicationDate: {PublicationDate}";
    }
}