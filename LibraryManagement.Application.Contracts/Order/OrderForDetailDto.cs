using LibraryManagement.Application.Contracts.Commons.Primitives;
using LibraryManagement.Application.Contracts.Interfaces;

namespace LibraryManagement.Application.Contracts.Order;

public class OrderForDetailDto : EntityDto<Guid>, IAuditableEntityDto
{
    public List<OrderItemForListDto> OrderItems { get; set; } = new();

    public string OrderStatus { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return $"Order has: {OrderItems.Count} items";
    }
}