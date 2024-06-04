namespace LibraryManagement.WebApi.Models;

public class AddItemToBasketRequest
{
    public int BookId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}