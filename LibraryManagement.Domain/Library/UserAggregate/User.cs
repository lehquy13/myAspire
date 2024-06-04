using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Exceptions.BookExceptions;
using LibraryManagement.Domain.Shared.Exceptions.UserExceptions;
using LibraryManagement.Domain.Shared.Utilities;

namespace LibraryManagement.Domain.Library.UserAggregate;

public sealed class User : AggregateRoot<IdentityGuid>, IAuditableEntity
{
    private string _name = string.Empty;
    private string _email = string.Empty;
    private List<Book> _favouriteBooks = new();
    private List<Book> _wishLists = new();
    private DateTime _updateAt = DateTime.Now;

    private User()
    {
        Id = IdentityGuid.Create();
    }
    
    

    public User(string email, string name, Address address)
    {
        Id = IdentityGuid.Create();
        Name = name;
        Address = address;
        Email = email;
    }

    public string Name
    {
        get => _name;
        internal set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidUserNameException();
            }

            _name = value;
        }
    }

    public Address Address { get; set; } = new();

    public string Email
    {
        get => _email;
        set
        {
            if (Helper.CheckValidEmail(value) == false)
            {
                throw new InvalidUserEmailException();
            }

            _email = value;
        }
    }

    public IdentityUser IdentityUser { get; private set; } = null!;

    public IReadOnlyCollection<Book> FavouriteBooks => _favouriteBooks.AsReadOnly();

    public IReadOnlyCollection<Book> WishLists => _wishLists.AsReadOnly();

    public DateTime CreatedAt { get; init; } = DateTime.Now;

    public DateTime UpdatedAt
    {
        get => _updateAt;
        set
        {
            if (value < CreatedAt)
            {
                throw new InvalidUserUpdateAtException();
            }

            _updateAt = value;
        }
    }

    public void AddFavoriteBook(List<Book> books)
    {
        if (books.Count == 0)
        {
            throw new CantAddEmptyListToFavourite();
        }
        
        List<Book> toAddBooks = books.Where(book => !_favouriteBooks.Contains(book)).ToList();

        if (toAddBooks.Count == 0)
        {
            throw new IsLikedBookException();
        }

        _favouriteBooks.AddRange(toAddBooks);
    }

    public void RemoveFavoriteBook(List<Book> books)
    {
        foreach (var book in books)
        {
            _favouriteBooks.Remove(book);
        }
    }

    public void AddToWishList(List<Book> books)
    {
        if (books.Count == 0)
        {
            throw new CantAddEmptyListToWistlist();
        }
        
        List<Book> toAddBooks = books.Where(book => !_wishLists.Contains(book)).ToList();

        if (toAddBooks.Count == 0)
        {
            throw new IsAlreadyInWishListException();
        }

        _wishLists.AddRange(toAddBooks);
    }

    public void RemoveFromWishList(List<Book> books)
    {
        foreach (var book in books)
        {
            _wishLists.Remove(book);
        }
    }
}