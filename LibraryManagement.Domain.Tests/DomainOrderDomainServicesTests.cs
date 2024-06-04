using LibraryManagement.Domain.DomainServices;
using LibraryManagement.Domain.DomainServices.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Specifications.Books;
using LibraryManagement.Domain.Specifications.Orders;
using LibraryManagement.Test.Shared;
using Moq;

namespace LibraryManagement.Domain.Tests;

public class DomainOrderDomainServicesTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IBasketRepository> _basketRepositoryMock = new();
    private readonly Mock<IIdentityRepository> _identityRepositoryMock = new();
    private readonly Mock<IAppLogger<OrderDomainServices>> _loggerMock = new();

    private readonly OrderDomainServices _bookDomainServices;

    public DomainOrderDomainServicesTests()
    {
        _bookDomainServices = new OrderDomainServices(
            _basketRepositoryMock.Object,
            _bookRepositoryMock.Object,
            _orderRepositoryMock.Object,
            _loggerMock.Object,
            _identityRepositoryMock.Object
        );
    }

    [Test]
    public async Task CreateOrderAsync_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var customer = TestShared.Users[0];
        var basket = TestShared.Baskets[0];

        var mockBooks = TestShared.Books;

        _basketRepositoryMock.Setup(x => x.GetBasketByUserIdAsync(customer.Id)).ReturnsAsync(basket);

        _bookRepositoryMock.Setup(x =>
                x.GetAllListAsync(It.IsAny<BookListByIdQuerySpec>()))
            .ReturnsAsync(mockBooks);

        _orderRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Order>()));

        // Act
        var result = await _bookDomainServices.CreateOrderAsync(customer.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task CreateOrderAsync_ShouldReturnFail_WhenBasketNotFound()
    {
        // Arrange
        Guid fakeId = new Guid();
        IdentityGuid fakeUserId = IdentityGuid.Create(fakeId);
        Basket? basket = null;

        _basketRepositoryMock.Setup(x =>
                x.GetBasketByUserIdAsync(fakeUserId))
            .ReturnsAsync(basket);

        // Act
        var result = await _bookDomainServices.CreateOrderAsync(fakeUserId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.BasketNotFound));
    }
    
    [Test]
    public async Task CreateOrderAsync_ShouldReturnFail_WhenBasketCountIs0()
    {
        // Arrange
        Guid fakeId = new Guid();
        IdentityGuid fakeUserId = IdentityGuid.Create(fakeId);
        Basket? basket = new Basket(IdentityGuid.Create());

        _basketRepositoryMock.Setup(x =>
                x.GetBasketByUserIdAsync(fakeUserId))
            .ReturnsAsync(basket);

        // Act
        var result = await _bookDomainServices.CreateOrderAsync(fakeUserId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.BasketIsEmpty));
    }

    [Test]
    public async Task PurchaseOrder_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var customer = TestShared.IdentityUsers[0];
        var order = TestShared.Orders[0];

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<OrderBelongToUserSpec>())).ReturnsAsync(order);

        _identityRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.PurchaseOrder(customer.Id, TestShared.Orders[0].Id, PaymentMethod.Cash);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task PurchaseOrder_ShouldReturnFail_WhenCustomerOrderIsNull()
    {
        // Arrange
        var customer = TestShared.IdentityUsers[1];

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<OrderBelongToUserSpec>())).ReturnsAsync((Order)null);
        _identityRepositoryMock.Setup(irepo => irepo.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);
        // Act
        var result = await _bookDomainServices.PurchaseOrder(customer.Id, TestShared.Orders[0].Id, PaymentMethod.Cash);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.OrderNotFound));
    }

    [Test]
    public async Task PurchaseOrder_ShouldReturnFail_WhenCustomerBalanceIsInsufficient()
    {
        // Arrange // Do anh quoc pass dc cai test nay
        var customer = TestShared.IdentityUsers[1];
        var order = TestShared.Orders[1];

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<OrderBelongToUserSpec>())).ReturnsAsync(order);
        _identityRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.PurchaseOrder(customer.Id, TestShared.Orders[0].Id, PaymentMethod.Cash);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(result.DisplayMessage.Contains(DomainServiceErrors.InsufficientUserBalance));
    }
}