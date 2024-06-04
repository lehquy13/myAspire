using LibraryManagement.Domain.DomainServices;
using LibraryManagement.Domain.DomainServices.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Test.Shared;
using Moq;

namespace LibraryManagement.Domain.Tests;

public class DomainBasketDomainServicesTests
{
    private readonly Mock<IBasketRepository> _basketRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepository = new();
    private readonly Mock<IAppLogger<BasketDomainServices>> _loggerMock = new();

    private BasketDomainServices _basketDomainServices;

    public DomainBasketDomainServicesTests()
    {
        _basketDomainServices =
            new BasketDomainServices(_basketRepositoryMock.Object, _loggerMock.Object, _bookRepository.Object);
    }

    [Test]
    public async Task AddItemToBasket_ShouldReturnSuccess_WhenBasketIsValid()
    {
        // Arrange
        var id = IdentityGuid.Create();
        var catalogItemId = 1;
        var book = TestShared.Books[0];
        var price = 1.0m;
        var quantity = 1;
        var basket = new Basket(id);

        _basketRepositoryMock.Setup(x => x.GetBasketByUserIdAsync(id)).ReturnsAsync(basket);
        _bookRepository.Setup(x => x.GetByIdAsync(catalogItemId)).ReturnsAsync(book);

        // Act
        var result = await _basketDomainServices.AddItemToBasket(id, catalogItemId, price, quantity);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task AddItemToBasket_ShouldReturnSuccess_WhenBasketIsNotFound()
    {
        // Arrange
        var id = IdentityGuid.Create();
        var catalogItemId = 1;
        var book = TestShared.Books[0];
        var price = 1;
        var quantity = 1;
        Basket? basket = null;

        _basketRepositoryMock.Setup(x => x.GetBasketByUserIdAsync(id)).ReturnsAsync(basket);
        _bookRepository.Setup(x => x.GetByIdAsync(catalogItemId)).ReturnsAsync(book);
        _basketRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Basket>()));

        // Act
        var result = await _basketDomainServices.AddItemToBasket(id, catalogItemId, price, quantity);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task SetQuantities_ShouldReturnBasket_WhenBasketIsValid()
    {
        // Arrange
        var basketId = IdentityGuid.Create(Guid.NewGuid());
        var quantities = new Dictionary<int, int>()
        {
            { 1, 2 }
        };
        var basket = new Basket(basketId);
        basket.AddItem(1, 2);

        _basketRepositoryMock.Setup(x => x.GetBasketByUserIdAsync(basketId)).ReturnsAsync(basket);

        // Act
        var result = await _basketDomainServices.SetQuantities(basketId, quantities);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value.Items, Has.Count.EqualTo(basket.Items.Count));
        Assert.That(result.Value.Id, Is.EqualTo(basket.Id));
    }

    [Test]
    public async Task SetQuantities_ShouldReturnFail_WhenBasketIsNotFound()
    {
        // Arrange
        var basketId = IdentityGuid.Create(Guid.NewGuid());
        var quantities = new Dictionary<int, int>();
        Basket? basket = null;

        _basketRepositoryMock.Setup(x => x.GetBasketByUserIdAsync(basketId)).ReturnsAsync(basket);
        // Act
        var result = await _basketDomainServices.SetQuantities(basketId, quantities);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.BasketNotFound));
    }
}