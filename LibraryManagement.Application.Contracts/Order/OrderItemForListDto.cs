using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Order;

public class OrderItemForListDto : EntityDto<Guid>
{
    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
    public string Title { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"OrderItem has: {Quantity} of {Title}";
        ;
    }
}