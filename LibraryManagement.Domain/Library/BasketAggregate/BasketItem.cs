using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Exceptions.BasketExceptions;

namespace LibraryManagement.Domain.Library.BasketAggregate;

public class BasketItem : Entity<int>
{
    private int _quantity;

    public int Quantity
    {
        get => _quantity;
        internal set
        {
            if (value < 0)
            {
                throw new InvalidBasketItemQuantityException();
            }

            _quantity = value;
        }
    }

    public int BookId { get; private set; }

    public Book Book { get; private set; } = null!;

    public int BasketId { get; private set; }

    //This is the navigation property but it is required not to use
    public Basket Basket { get; private set; } = null!;

    public BasketItem(int bookId, int quantity)
    {
        BookId = bookId;
        Quantity = quantity;
    }

    private BasketItem()
    {
    }

    public void AddQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new InvalidBasketItemQuantityException();
        }

        Quantity += quantity;
    }
}