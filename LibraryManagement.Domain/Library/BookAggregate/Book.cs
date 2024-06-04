using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Exceptions.BookExceptions;

namespace LibraryManagement.Domain.Library.BookAggregate;

public class Book : AggregateRoot<int>
{
    private string _title = string.Empty;
    private int _quantity;
    private DateTime _publicationDate;
    private decimal _price;
    private decimal _currentPrice;
    private string _image = string.Empty;
    private List<Author> _authors = new();

    private Book()
    {
    }

    public static Book Create(
        string title,
        int quantity,
        string image,
        decimal price,
        decimal currentPrice,
        List<Author> authors,
        Genre genre,
        DateTime publishYear)
    {
        return new Book()
        {
            Title = title,
            Quantity = quantity,
            Authors = authors,
            Genre = genre,
            PublicationDate = publishYear,
            Image = image,
            Price = price,
            CurrentPrice = currentPrice
        };
    }

    public string Title
    {
        get => _title;

        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidBookTitleException();
            }

            _title = value;
        }
    }

    public int Quantity
    {
        get => _quantity;

        set
        {
            if (value < 0)
            {
                throw new InvalidBookQuantityException();
            }

            _quantity = value;
        }
    }

    public DateTime PublicationDate
    {
        get => _publicationDate;

        set
        {
            if (value > DateTime.Now)
            {
                throw new InvalidBookPublishYearException();
            }

            _publicationDate = value;
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new InvalidBookPriceException();
            }

            _price = value;
        }
    }

    public Genre Genre { get; set; }

    public List<Review> Reviews { get; private set; } = new();

    public List<Author> Authors
    {
        get => _authors;
        set
        {
            if (value.Count == 0)
            {
                throw new InvalidBookAuthorException();
            }

            _authors = value;
        }
    }

    //Navigation property
    public List<User> BookFancier { get; private set; } = null!;

    public string Image
    {
        get => _image;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidImageException();
            }

            _image = value;
        }
    }

    public decimal CurrentPrice
    {
        get => _currentPrice;
        set
        {
            if (value < 0)
            {
                throw new InvalidBookPriceException();
            }

            _currentPrice = value;
        }
    }

    public void AddReview(Review review)
    {
        Reviews.Add(review);
    }
}