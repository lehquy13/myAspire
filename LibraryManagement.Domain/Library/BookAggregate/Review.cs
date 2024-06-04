using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Exceptions.BookExceptions;
using LibraryManagement.Domain.Shared.Exceptions.ReviewExceptions;

namespace LibraryManagement.Domain.Library.BookAggregate;

public class Review : Entity<int>
{
    private string _title = string.Empty;
    private string _content = string.Empty;
    private int _rating = 5;
    private string? _image = string.Empty;

    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidReviewTitleException();
            }

            _title = value;
        }
    }

    public string Content
    {
        get => _content;
        set
        {
            if (string.IsNullOrEmpty(value) || value.Length < 10)
            {
                throw new InvalidReviewContentException();
            }

            _content = value;
        }
    }

    public DateTime ReviewDate { get; } = DateTime.Now;

    public bool IsLike { get; init; } = false;

    public int Rating
    {
        get => _rating;
        set
        {
            if (value < 1 || value > 5)
            {
                throw new InvalidReviewRatingException();
            }

            _rating = value;
        }
    }

    public IdentityGuid UserId { get; protected set; } = null!;

    // Please find some other way to handle this.
    // I do make it public in order to unit test. It's better be private set
    public User User { get; set; } = null!;

    public int BookId { get; private set; }

    //This is the navigation property but it is required not to use
    public Book Book { get; private set; } = null!;

    public string? Image // Public get set due to a review can have image or not
    {
        get => _image;
        set => _image = value;
    }

    private Review()
    {
    }

    public Review(string title, string content, int rating, string image, bool isLike,
        IdentityGuid userId)
    {
        Title = title;
        Content = content;
        Rating = rating;
        IsLike = isLike;
        UserId = userId;
        Image = image;
    }
}