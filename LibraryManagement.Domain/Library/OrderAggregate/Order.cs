using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Enums;

namespace LibraryManagement.Domain.Library.OrderAggregate;

public sealed class Order : AggregateRoot<OrderGuid>, IAuditableEntity
{
    private List<OrderItem> _orderItems = new();

    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public IdentityGuid UserId { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public DateTime CreatedAt { get; } = DateTime.Now;

    public DateTime UpdatedAt { get; } = DateTime.Now;

    public PaymentMethod PaymentMethod { get; private set; }

    public OrderStatus OrderStatus { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice => _orderItems.Sum(x => x.UnitPrice * x.Quantity);

    private Order()
    {
    }

    public Order(IdentityGuid userId)
    {
        Id = OrderGuid.Create();
        UserId = userId;
    }

    public void PurchaseOrder(PaymentMethod paymentMethod)
    {
        PaymentMethod = paymentMethod;
        OrderStatus = OrderStatus.Completed;
    }

    public void AddOrderItems(List<OrderItem> items)
    {
        _orderItems.AddRange(items);
    }

    public void AddOrderItems(params OrderItem[] items)
    {
        _orderItems.AddRange(items);
    }

    public void UpdateOrderStatus(OrderStatus orderStatus)
    {
        OrderStatus = orderStatus;
    }
}

