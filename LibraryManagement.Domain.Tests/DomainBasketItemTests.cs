using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Shared.Exceptions.BasketExceptions;

namespace LibraryManagement.Domain.Tests;

public class DomainBasketItemTests
{
    [Test]
    public void BasketItem_WithValidBookIdAndQuantity_ShouldCreateBasketItem()
    {
        // Arrange
        int bookId = 1;
        int quantity = 3;

        // Act
        var basketItem = new BasketItem(bookId, quantity);

        // Assert
        Assert.That(basketItem.BookId, Is.EqualTo(bookId));
        Assert.That(basketItem.Quantity, Is.EqualTo(quantity));
    }

    [Test]
    public void BasketItem_WithInvalidQuantity_ShouldThrowInvalidOrderItemQuantityException()
    {
        // Arrange
        int bookId = 1;
        int invalidQuantity = -1;

        // Act and Assert
        Assert.Throws<InvalidBasketItemQuantityException>(() =>
        {
            var basketItem = new BasketItem(bookId, invalidQuantity);
        });
    }


    [Test]
    public void AddQuantity_WithValidQuantity_ShouldIncreaseQuantity()
    {
        // Arrange
        var basketItem = new BasketItem(1, 3);
        int quantityToAdd = 2;
        int expectedQuantity = 5;

        // Act
        basketItem.AddQuantity(quantityToAdd);

        // Assert
        Assert.That(basketItem.Quantity, Is.EqualTo(expectedQuantity));
    }

    [Test]
    public void AddQuantity_WithNegativeQuantity_ShouldThrowInvalidOrderItemQuantityException()
    {
        // Arrange
        var basketItem = new BasketItem(1, 3);
        int quantityToAdd = -2;

        // Act and Assert
        Assert.Throws<InvalidBasketItemQuantityException>(() => basketItem.AddQuantity(quantityToAdd));
    }

    [Test]
    public void AddQuantity_WithZeroQuantity_ShouldThrowInvalidOrderItemQuantityException()
    {
        // Arrange

        var basketItem = new BasketItem(1, 3);
        int quantityToAdd = 0;

        // Act and Assert
        Assert.Throws<InvalidBasketItemQuantityException>(() => basketItem.AddQuantity(quantityToAdd));
    }

    [Test]
    public void AddQuantity_WithPositiveQuantity_ShouldIncreaseQuantity()
    {
        // Arrange
        var basketItem = new BasketItem(1, 3);
        int quantityToAdd = 2;
        int expectedQuantity = 5;

        // Act
        basketItem.AddQuantity(quantityToAdd);

        // Assert
        Assert.That(basketItem.Quantity, Is.EqualTo(expectedQuantity));
    }
}