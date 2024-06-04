using LibraryManagement.Domain.DomainServices;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Test.Shared;
using Moq;

namespace LibraryManagement.Domain.Tests;

public class DomainBookDomainServicesTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IAppLogger<BookDomainServices>> _loggerMock = new();

    private readonly BookDomainServices _bookDomainServices;

    public DomainBookDomainServicesTests()
    {
        _bookDomainServices = new BookDomainServices(
            _bookRepositoryMock.Object,
            _userRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Test]
    public async Task AddFavouriteBook_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var customer = TestShared.Users[0];
        var book = TestShared.Books[0];

        _bookRepositoryMock.Setup(x => x.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _userRepositoryMock.Setup(x => x.GetFullById(customer.Id)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.AddFavouriteBook(customer.Id, book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task AddFavouriteBook_ShouldReturnFail_WhenCustomerIsNotFound()
    {
        // Arrange
        User? customer = null;
        var fakeId = IdentityGuid.Create();
        var book = TestShared.Books[0];

        _bookRepositoryMock.Setup(x => x.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(fakeId)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.AddFavouriteBook(fakeId, book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
    }

    [Test]
    public async Task AddFavouriteBook_ShouldReturnFail_WhenBookIsNotFound()
    {
        // Arrange
        User customer = TestShared.Users[0];
        int fakeId = 9999;
        Book? book = null;

        _bookRepositoryMock.Setup(x => x.GetByIdAsync(fakeId)).ReturnsAsync(book);
        _userRepositoryMock.Setup(x => x.GetByIdAsync(customer.Id)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.AddFavouriteBook(customer.Id, fakeId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
    }

    [Test]
    public async Task AddItemToWishList_ShouldReturnSuccess_WhenValid()
    {
        // Arrange
        var customer = TestShared.Users[0];
        var book = TestShared.Books[0];

        _bookRepositoryMock.Setup(x => x.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _userRepositoryMock.Setup(x => x.GetFullById(customer.Id)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.AddItemToWishList(customer.Id, book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task AddItemToWishList_ShouldReturnFail_WhenCustomerIsNotFound()
    {
        // Arrange
        User? customer = null;
        var fakeId = IdentityGuid.Create();
        var book = TestShared.Books[0];

        _bookRepositoryMock.Setup(x => x.GetByIdAsync(book.Id)).ReturnsAsync(book);
        _userRepositoryMock.Setup(x => x.GetFullById(fakeId)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.AddItemToWishList(fakeId, book.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
    }

    [Test]
    public async Task AddItemToWishList_ShouldReturnFail_WhenBookIsNotFound()
    {
        // Arrange
        User customer = TestShared.Users[0];
        int fakeId = 9999;
        Book? book = null;

        _bookRepositoryMock.Setup(x => x.GetByIdAsync(fakeId)).ReturnsAsync(book);
        _userRepositoryMock.Setup(x => x.GetFullById(customer.Id)).ReturnsAsync(customer);

        // Act
        var result = await _bookDomainServices.AddItemToWishList(customer.Id, fakeId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsFalse(result.IsSuccess);
    }
}