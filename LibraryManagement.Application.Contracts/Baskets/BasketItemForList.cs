using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Baskets;

public class BasketItemForList : EntityDto<int>
{
    public BookForListDto BookForListDto { get; set; } = null!;
    public int Quantity { get; set; }
    
    public override string ToString()
    {
        return $"Item has: {Quantity} of {BookForListDto}";
    }
}