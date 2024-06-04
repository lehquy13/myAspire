using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Primitives;

namespace LibraryManagement.Domain.Library.BasketAggregate;

public class Basket : AggregateRoot<int>
{
    private readonly List<BasketItem> _items = new();

    public IReadOnlyList<BasketItem> Items => _items.AsReadOnly();

    public IdentityGuid UserId { get; private set; } = null!;

    //This is the navigation property but it is required not to use
    public User User { get; private set; } = null!;

    public void AddItem(int bookItemId, int quantity = 1)
    {
        if (Items.All(i => i.BookId != bookItemId))
        {
            _items.Add(new BasketItem(bookItemId, quantity));
            return;
        }

        var existingItem = Items.First(i => i.BookId == bookItemId);
        existingItem.AddQuantity(quantity);
    }

    public void RemoveEmptyItems()
    {
        _items.RemoveAll(i => i.Quantity == 0);
    }

    public void ClearItems()
    {
        _items.Clear();
    }

    private Basket()
    {
    }

    public Basket(IdentityGuid userId)
    {
        UserId = userId;
    }
}