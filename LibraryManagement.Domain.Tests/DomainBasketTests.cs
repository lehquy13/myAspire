using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;

namespace LibraryManagement.Domain.Tests;

public class DomainBasketTests
{
    private readonly IdentityGuid _userId = IdentityGuid.Create();

    [Test]
    public void Basket_WithValidIdentityGuid_ShouldCreateBasket()
    {
        // Act
        var basket = new Basket(_userId);

        // Assert
        Assert.That(basket.Items.Count, Is.EqualTo(0));
        Assert.That(basket.UserId, Is.EqualTo(_userId));
    }

    [Test]
    public void Basket_WithEmptyItem_WhenRemoveEmptyItems_ShouldBeEmpty()
    {
        // Arrange
        var basket = new Basket(_userId);
        basket.AddItem(1, 0);
        var currentItemsCount = basket.Items.Count;

        // Act
        basket.RemoveEmptyItems();

        // Act and Assert
        Assert.That(currentItemsCount - 1, Is.EqualTo(0));
        Assert.That(basket.UserId, Is.EqualTo(_userId));
    }

    [Test]
    public void Basket_WithBook_ShouldAddNewBook()
    {
        // Arrange
        var basket = new Basket(_userId);
        var currentItemsCount = basket.Items.Count;

        // Act
        basket.AddItem(1, 1);

        // Assert
        Assert.That(basket.Items.Count, Is.EqualTo(currentItemsCount + 1));
    }

    [Test]
    public void Basket_WithExistBook_ShouldIncreaseBookCount()
    {
        // Arrange
        var basket = new Basket(_userId);
        basket.AddItem(1, 1);
        var currentItemsCount = basket.Items[0].Quantity;

        // Act
        basket.AddItem(1, 100);

        // Assert
        Assert.That(basket.Items[0].Quantity, Is.EqualTo(currentItemsCount + 100));
    }
}