using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;
using LibraryManagement.Test.Shared;

namespace LibraryManagement.Domain.Tests;

public class DomainOrderTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Constructor_WithValidArguments_ShouldInitializeOrder()
    {
        // Arrange
        var userId = IdentityGuid.Create();

        // Act
        var order = new Order(userId);

        // Assert
        Assert.NotNull(order.Id);
        Assert.That(order.UserId, Is.EqualTo(userId));
        Assert.IsEmpty(order.OrderItems);
        Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.Pending));
        Assert.That(order.TotalPrice, Is.EqualTo(0));
        Assert.GreaterOrEqual(DateTime.Now, order.CreatedAt);
    }

    [Test]
    public void Constructor_WithOrderItems_ShouldInitializeOrderWithItems()
    {
        // Arrange
        var userId = IdentityGuid.Create();
        var bookToBuy = TestShared.Books[0];
        var order = new Order(userId);
        var quantity = 11;
        var price = 11;
        var orderItems = new List<OrderItem>
        {
            new(bookToBuy.Id, quantity, price)
        };

        order.AddOrderItems(orderItems);

        // Act
        // Assert
        Assert.NotNull(order.Id);
        Assert.That(order.UserId, Is.EqualTo(userId));
        Assert.IsNotEmpty(order.OrderItems);
        CollectionAssert.AreEqual(orderItems, order.OrderItems.ToList());
        Assert.GreaterOrEqual(DateTime.Now, order.CreatedAt);
        Assert.GreaterOrEqual(quantity * price, order.TotalPrice);
    }

    [Test]
    public void PurchaseOrder_WithValidPaymentMethod_ShouldSetPaymentMethodAndIsPurchase()
    {
        // Arrange
        var userId = IdentityGuid.Create();
        var order = new Order(userId);
        var paymentMethod = PaymentMethod.Card;

        // Act
        order.PurchaseOrder(paymentMethod);

        // Assert
        Assert.That(order.PaymentMethod, Is.EqualTo(paymentMethod));
        Assert.That(order.OrderStatus, Is.EqualTo(OrderStatus.Completed));
    }

    [Test]
    public void AddOrderItems_WithItems_ShouldAddOrderItems()
    {
        // Arrange
        var userId = IdentityGuid.Create();
        var order = new Order(userId);
        var orderItems = new List<OrderItem>
        {
            new(1, 10, 11),
            new(2, 15, 12)
        };

        // Act
        order.AddOrderItems(orderItems);

        // Assert
        CollectionAssert.AreEqual(orderItems, order.OrderItems.ToList());
    }

    [Test]
    public void CreateOrderItem_WithValidItems_ShouldSuccess()
    {
        // Arrange
        var bookId = 1;
        var quantity = 10;
        var price = 11;
        
        // Act
        var orderItem = new OrderItem(bookId, quantity, price);
        
        // Assert
        Assert.That(orderItem.BookId, Is.EqualTo(bookId));
        Assert.That(orderItem.Quantity, Is.EqualTo(quantity));
        Assert.That(orderItem.UnitPrice, Is.EqualTo(price));
    }
    
    [Test]
    public void CreateOrderItem_WithNegativeQuantity_ShouldThrowException()
    {
        //Arrange
        var bookId = 1;
        var quantity = -10;
        var price = 11;
        
        //Act
        
        //
        Assert.Throws<InvalidOrderItemQuantityException>(() =>
        {
            var orderItem = new OrderItem(bookId, quantity, price);
        });
    }
    
    [Test]
        public void CreateOrderItem_WithNegativePrice_ShouldThrowException()
        {
            //Arrange
            var bookId = 1;
            var quantity = 10;
            var price = -11;
            
            //Act
            
            //
            Assert.Throws<InvalidOrderItemUnitPriceException>(() =>
            {
                var orderItem = new OrderItem(bookId, quantity, price);
            });
        }
}