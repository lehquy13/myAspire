using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Baskets;

public class BasketForDetailDto : EntityDto<int>
{
    public List<BasketItemForList> Items { get; set; } = new();

    public decimal TotalPrice { get; set; } = 0;

    public override string ToString()
    {
        return $"Basket has: {Items.Count} items";
    }
}