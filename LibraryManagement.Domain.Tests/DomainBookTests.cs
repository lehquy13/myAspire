using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

namespace LibraryManagement.Domain.Tests;

public class DomainBookTests
{
    private readonly string _title = "Test Book";
    private readonly string _image = "Image of Good Book";
    private readonly int _quantity = 5;
    private readonly decimal _price = 5m;
    private readonly decimal _currentPrice = 5m;
    private readonly DateTime _publishYear = new DateTime(2015, 12, 25);
    private readonly Author _author = new Author("Test Author");
    private readonly Genre _genre = Genre.Adventure;

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Book_WithValidProperties_ShouldSetPropertiesCorrectly()
    {
        // Arrange

        // Act
        Book book = Book.Create(_title, _quantity, _image, _price, _currentPrice,
            new List<Author>() { _author }, _genre, _publishYear);

        // Assert
        Assert.That(book.Title, Is.EqualTo(_title));
        Assert.That(book.Authors[0].Id, Is.EqualTo(_author.Id));
        Assert.That(book.Quantity, Is.EqualTo(_quantity));
        Assert.That(book.PublicationDate, Is.EqualTo(_publishYear));
        Assert.That(book.Image, Is.EqualTo(_image));
        Assert.That(book.CurrentPrice, Is.EqualTo(_currentPrice));
        Assert.That(book.Price, Is.EqualTo(_price));
        Assert.That(book.Genre, Is.EqualTo(_genre));
    }

    [Test]
    public void Book_WithEmptyTitle_ShouldThrowInvalidBookTitleException()
    {
        // Arrange
        string title = "";

        // Act & Assert
        Assert.Throws<InvalidBookTitleException>(() =>
        {
            Book.Create(title, _quantity, _image, _price, _currentPrice, new List<Author>() { _author },
                _genre, _publishYear);
        });
    }

    [Test]
    public void Book_WithEmptyAuthorList_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<InvalidBookAuthorException>(() =>
        {
             Book.Create(_title, _quantity, _image, _price, _currentPrice, new List<Author>(), _genre,
                _publishYear);
        });
    }

    [Test]
    public void Book_WithInvalidQuantity_ShouldThrowInvalidBookQuantityException()
    {
        // Arrange
        int quantity = -5;

        // Act & Assert
        Assert.Throws<InvalidBookQuantityException>(() =>
        {
            
                Book.Create(_title, quantity, _image, _price, _currentPrice,
                    new List<Author>() { _author }, _genre, _publishYear);
        });
    }

    [Test]
    public void Book_WithInvalidPublishYear_ShouldThrowInvalidBookPublishYearException()
    {
        // Arrange
        DateTime publishYear = new DateTime(3000, 12, 25);

        // Act & Assert
        Assert.Throws<InvalidBookPublishYearException>(() =>
        {
             Book.Create(_title, _quantity, _image, _price, _currentPrice,
                new List<Author>() { _author }, _genre, publishYear);
        });
    }
    
    [Test]
    public void Book_WithInvalidPrice_ShouldThrowInvalidBookPriceException()
    {
        // Arrange
        decimal price = -5m;

        // Act & Assert
        Assert.Throws<InvalidBookPriceException>(() =>
        {
             Book.Create(_title, _quantity, _image, price, _currentPrice,
                new List<Author>() { _author }, _genre, _publishYear);
        });
    }
    
    [Test]
    public void Book_WithInvalidCurrentPrice_ShouldThrowInvalidBookCurrentPriceException()
    {
        // Arrange
        decimal currentPrice = -5m;

        // Act & Assert
        Assert.Throws<InvalidBookPriceException>(() =>
        {
             Book.Create(_title, _quantity, _image, _price, currentPrice,
                new List<Author>() { _author }, _genre, _publishYear);
        });
    }
}