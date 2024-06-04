using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Shared.Exceptions.AuthorExceptions;

namespace LibraryManagement.Domain.Tests;

public class DomainAuthorTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void Author_WithValidName_ShouldCreateAuthor()
    {
        // Arrange
        string name = "John Doe";

        // Act
        var author = new Author(name);

        // Assert
        Assert.That(author.Name, Is.EqualTo(name));
    }

    [Test]
    public void Author_WithEmptyName_ShouldThrowInvalidAuthorNameException()
    {
        // Arrange
        string name = string.Empty;

        // Act and Assert
        Assert.Throws<InvalidAuthorNameException>(() =>
        {
            var author = new Author(name);
        });
    }
}