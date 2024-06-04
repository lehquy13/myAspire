using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Exceptions.UserExceptions;
using LibraryManagement.Test.Shared;

namespace LibraryManagement.Domain.Tests;

public class DomainUserTests
{
    private readonly DateTime _publishYear = new DateTime(2015, 12, 25);
    private readonly string _image = "Image of Good Book";

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AddFavoriteBook_WhenBookIsNotAlreadyLiked_ShouldAddToFavoriteBooks()
    {
        // Arrange
        var user = TestShared.Users[0];
        var newBookToAdd =
            Book.Create("Book 1", 10, _image, 100, 90,
                new List<Author>() { TestShared.Authors[0] }, Genre.SelfHelp, _publishYear);

        // Act
        user.AddFavoriteBook(
            new()
            {
                newBookToAdd
            }
        );

        // Assert
        Assert.IsTrue(user.FavouriteBooks.Contains(newBookToAdd));
    }

    [Test]
    public void AddFavoriteBook_WhenBookIsAlreadyLiked_ShouldThrowIsLikedBookException()
    {
        // Arrange
        var user = TestShared.Users[0];
        var book = TestShared.Books[2];
        user.AddFavoriteBook(
            new()
            {
                book
            }
        ); // Add book once

        // Act and Assert
        Assert.Throws<IsLikedBookException>(() =>
            user.AddFavoriteBook(
                new()
                {
                    book
                }
            )); // Try to add the same book again
    }

    [Test]
    public void RemoveFavoriteBook_WhenBookIsLiked_ShouldRemoveFromFavoriteBooks()
    {
        // Arrange
        var user = TestShared.Users[0];
        var book = TestShared.Users[0].FavouriteBooks.First();
        var count = TestShared.Users[0].FavouriteBooks.Count;
        
        // Act
        user.RemoveFavoriteBook(
            new()
            {
                book
            }
        );
        
        // Assert
        Assert.That(user.FavouriteBooks.Count, Is.EqualTo(count - 1));
    }

    [Test]
    public void RemoveFavoriteBook_WhenBookIsNotLiked_ShouldNotThrowException()
    {
        // Arrange
        var user = TestShared.Users[0];
        var book = TestShared.Books[0];

        // Act and Assert (No exception should be thrown)
        Assert.DoesNotThrow(() => user.RemoveFavoriteBook(
            new()
            {
                book
            }
        ));
    }
    
    [Test]
    public void CreateUser_WhenUpdatedAtIsLessThanCreatedAt_ShouldThrowException()
    {
        // Arrange
        var user = TestShared.Users[0];

        // Act and Assert (No exception should be thrown)
        Assert.Throws<InvalidUserUpdateAtException>(() => user.UpdatedAt = new DateTime(100, 1, 1));
    }
    
    [Test]
    public void AddToWishList_WhenUpdatedAtIsLessThanCreatedAt_ShouldThrowException()
    {
        // Arrange
        var user = TestShared.Users[0];
        var book = TestShared.Books[0];

        // Act and Assert (No exception should be thrown)
        Assert.Throws<InvalidUserUpdateAtException>(() => user.UpdatedAt = new DateTime(100, 1, 1));
    }

    [Test]
    public void AddFavoriteBook_WithInvalidEmail_ShouldThrowInvalidUserEmailException()
    {
        // Arrange
        string invalidEmail = "invalid-email";
        // Act and Assert
        Assert.Throws<InvalidUserEmailException>(() =>
        {
            var user = new User(invalidEmail, "John Doe", new Address());
        });
    }
}