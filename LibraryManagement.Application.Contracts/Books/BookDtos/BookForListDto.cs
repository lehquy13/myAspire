using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Books.BookDtos;

public class BookForListDto : EntityDto<int>
{
    public string Title { get; set; } = null!;
    
    public List<string> AuthorNames { get; set; } = null!;

    public string Genre { get; set; } = null!;
    
    public string Image { get; set; } = null!;

    public int Quantity { get; set; }
    
    public DateTime PublicationDate { get; set; }

   
    public override string ToString()
    {
        return $"Id: {Id}\n" +
               $"Title: {Title} \n" +
               $"Author {AuthorNames} \n" +
               $"{Genre} \n" +
               $"Quantity: {Quantity} \n" +
               $"PublicationDate: {PublicationDate}";
    }
}