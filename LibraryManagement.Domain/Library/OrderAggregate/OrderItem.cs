using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;

namespace LibraryManagement.Domain.Library.OrderAggregate;

public sealed class OrderItem : Entity<OrderDetailGuid>
{
    private int _quantity;
    private decimal _unitPrice;

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
            {
                throw new InvalidOrderItemQuantityException();
            }

            _quantity = value;
        }
    }
    
    public decimal UnitPrice
    {
        get => _unitPrice;
        set
        {
            if (value < 0)
            {
                throw new InvalidOrderItemUnitPriceException();
            }
            _unitPrice = value;
        }
    }

    public int BookId { get; private set; }
    
    // Please find some other way to handle this.
    // I do make it public in order to unit test. It's better be private set
    public Book Book { get; set; } = null!; 

    public OrderGuid OrderId { get; set; } = null!;

    private OrderItem() => Id = OrderDetailGuid.Create();

    public OrderItem(int bookId, int quantity, decimal unitPrice)
    {
        Id = OrderDetailGuid.Create();
        BookId = bookId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}