namespace LibraryManagement.WebApi.Models;

public class UpdateItemQuantitiesRequest
{
    public int BookId { get; set; }
    public int Quantity { get; set; }
}