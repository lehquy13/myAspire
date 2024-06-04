using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Exceptions.BookExceptions;
using LibraryManagement.Domain.Shared.Exceptions.ReviewExceptions;

namespace LibraryManagement.Domain.Tests;

public class DomainReviewTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SetTitle_ValidTitle_ShouldSetTitle()
    {
        // Arrange
        string title = "Great Book";
        var review = new Review(title, "Test content 10 characs", 5, "fakeImage", true, IdentityGuid.Create());

        // Act
        review.Title = title;

        // Assert
        Assert.That(review.Title, Is.EqualTo(title));
    }

    [Test]
    public void SetTitle_NullOrEmptyTitle_ShouldThrowInvalidReviewTitleException()
    {
        // Arrange
        var review = new Review("test title", "Test content 10 characs", 5, "fakeImage", true, IdentityGuid.Create());

        // Assert
        Assert.Throws<InvalidReviewTitleException>(() => review.Title = null);
        Assert.Throws<InvalidReviewTitleException>(() => review.Title = string.Empty);
    }

    [Test]
    public void SetContent_ValidContent_ShouldSetContent()
    {
        // Arrange
        string content = "This book is amazing!";
        var review = new Review("Title", content, 5, "fakeImage", true, IdentityGuid.Create());

        // Act
        review.Content = content;

        // Assert
        Assert.That(review.Content, Is.EqualTo(content));
    }


    [Test]
    public void Review_WithInvalidContent_ShouldThrowInvalidReviewContentException()
    {
        // Arrange
        IdentityGuid userId = IdentityGuid.Create();

        // Act and Assert
        Assert.Throws<InvalidReviewContentException>(() =>
            new Review("Valid Title", "Short", 5, "fakeImage", true, userId));
    }

    [Test]
    public void Review_WithInvalidRating_ShouldThrowInvalidReviewRatingException()
    {
        // Arrange
        IdentityGuid userId = IdentityGuid.Create();

        // Act and Assert
        Assert.Throws<InvalidReviewRatingException>(() =>
            new Review("Valid Title", "Valid Content", 6, "fakeImage", true, userId));
    }

    [Test]
    public void Review_WithInvalidImage_ShouldThrowInvalidImageRatingException()
    {
        // Arrange
        IdentityGuid userId = IdentityGuid.Create();

        // Act and Assert
        Assert.Throws<InvalidReviewRatingException>(() =>
            new Review("Valid Title", "Valid Content", 6, "fakeImage", true, userId));
    }

    [Test]
    public void Review_WithValidData_ShouldHaveCorrectProperties()
    {
        // Arrange
        IdentityGuid userId = IdentityGuid.Create();
        string title = "Valid Title";
        string content = "Valid Content";
        int rating = 5;
        bool isLike = true;

        // Act
        var review = new Review(title, content, rating, "fakeImage", isLike, userId);

        // Assert
        Assert.That(review.Title, Is.EqualTo(title));
        Assert.That(review.Content, Is.EqualTo(content));
        Assert.That(review.Rating, Is.EqualTo(rating));
        Assert.That(review.IsLike, Is.EqualTo(isLike));
        Assert.That(review.UserId, Is.EqualTo(userId));
    }
}