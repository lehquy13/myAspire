using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.Application.Contracts.Users;
using LibraryManagement.Application.Mapping;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Shared.Enums;
using Mapster;
using MapsterMapper;

namespace LibraryManagement.Test.Shared;

public static class TestShared
{
    private static readonly Mapper Mapper;

    public static List<Author> Authors;

    public static List<Book> Books = null!;

    public static List<BookForListDto> BookForListDtos = null!;

    public static List<BookForDetailDto> BookForDetailDtos = null!;

    public static List<User> Users = null!;

    public static List<UserForListDto> UserForListDtos = null!;

    public static List<UserForDetailDto> UserForDetailDtos = null!;

    public static List<IdentityRole> IdentityRoles = null!;

    public static List<IdentityUser> IdentityUsers = null!;

    public static List<Address> Addresses = null!;

    public static List<Order> Orders = null!;

    public static List<Basket> Baskets = null!;

    static TestShared()
    {
        //Config instance
        var config = TypeAdapterConfig.GlobalSettings;
        config.Apply(new BookMappingConfig());
        config.Apply(new UserMappingConfig());

        Mapper = new(config);

        Authors = new()
        {
            new Author("Author 1"),
            new Author("Author 2"),
            new Author("Author 3")
        };
        UserInitial();

        BookInitial();

        OrderInitial();
    }

    private static void OrderInitial()
    {
        //Set up Order
        Orders = new List<Order>()
        {
            new(Users[0].Id),
            new(Users[1].Id),
            new(Users[2].Id),
            new(Users[1].Id),
            new(Users[2].Id),
        };
        Orders[1].AddOrderItems(
            new OrderItem(Books[1].Id, 2, Books[1].Price)
            {
                Book = Books[1]
            }
        );
    }

    private static void BookInitial()
    {
        DateTime date = new DateTime(2015, 12, 25);
        string image = "Image of Good Book";

        Books = new()
        {
            Book.Create("Book 1", 10, image, 100, 90,
                new List<Author>() { Authors[0] }, Genre.SelfHelp, date),
            Book.Create("Book 2", 10, image, 101, 91,
                new List<Author>() { Authors[1] }, Genre.Science, date),
            Book.Create("Book 3", 30, image, 102, 92,
                new List<Author>() { Authors[2] }, Genre.Biography, date),
            Book.Create("Book 4", 40, image, 103, 93,
                new List<Author>() { Authors[0], Authors[1] }, Genre.Adventure, date),
            Book.Create("Book 5", 50, image, 104, 94,
                new List<Author>() { Authors[1], Authors[2] }, Genre.Fantasy, date),
            Book.Create("Book 6", 64, image, 105, 95,
                new List<Author>() { Authors[0], Authors[2] }, Genre.Science, date),
        };

        Books[0].AddReview(new Review("Review 1", "Content with many characters 1", 5, "fakeImage", true,
            Users[0].Id)
        {
            User = Users[1]
        });
        Books[0].AddReview(new Review("Review 2", "Content with many characters 2", 4, "fakeImage", true,
            Users[1].Id)
        {
            User = Users[1]
        });
        Books[0].AddReview(new Review("Review 3", "Content with many characters 3", 3, "fakeImage", true,
            Users[2].Id)
        {
            User = Users[2]
        });

        Books[0].AddReview(new Review("Review 4", "Content with many characters 4", 5, "fakeImage", true,
            Users[0].Id));
        Books[1].AddReview(new Review("Review 5", "Content with many characters 5", 4, "fakeImage", true,
            Users[1].Id));
        Books[1].AddReview(new Review("Countent 6", "Content with many characters 6", 3, "fakeImage", true,
            Users[2].Id));

        Books[5].AddReview(new Review("Review 7", "Content with many characters 7", 5, "fakeImage", true,
            Users[0].Id));

        BookForListDtos = Mapper.Map<List<BookForListDto>>(Books);
        BookForDetailDtos = Mapper.Map<List<BookForDetailDto>>(Books);

        //Generate basket
        Baskets = new List<Basket>();
        foreach (var user in Users)
        {
            Baskets.Add(new Basket(user.Id));
        }

        //Generate basket item
        foreach (var basket in Baskets)
        {
            basket.AddItem(Books[0].Id, 3);
            basket.AddItem(Books[1].Id, 2);
            basket.AddItem(Books[2].Id, 5);
        }
    }

    private static void UserInitial()
    {
        Addresses = new List<Address>()
        {
            new()
            {
                City = "City 1",
                Country = "Country 1",
            },
            new()
            {
                City = "City 2",
                Country = "Country 2",
            },
            new()
            {
                City = "City 3",
                Country = "Country 3",
            },
        };

        Users = new()
        {
            new("User1@mail.com", "User 1", Addresses[0]),
            new("User2@mail.com", "User 2", Addresses[1]),
            new("User3@mail.com", "User 3", Addresses[2]),
            new("User4@mail.com", "User 4", Addresses[2]),
        };

        IdentityRoles = new List<IdentityRole>()
        {
            new()
            {
                Name = "Role1"
            }
        };

        var identityUser = new IdentityUser("01123", "1231231");
        var identityUser1 = new IdentityUser("01123123", "1231231");

        IdentityUsers = new List<IdentityUser>();
        identityUser.WithUser(Users[0]);
        identityUser.WithRole(IdentityRoles[0]);
        identityUser1.WithUser(Users[1]);
        identityUser1.WithRole(IdentityRoles[0]);
        identityUser.Deposit(1000000);
        IdentityUsers.Add(identityUser);
        IdentityUsers.Add(identityUser1);

        DateTime date = new DateTime(2015, 12, 25);
        string image = "Image of Good Book";

        Users[0].AddFavoriteBook(new()
        {
            Book.Create("Book 6", 5, image, 100, 90,
                new List<Author>() { Authors[1], Authors[2] }, Genre.Fantasy, date),
            Book.Create("Book 7", 6, image, 100, 90,
                new List<Author>() { Authors[0], Authors[2] }, Genre.Science, date)
        });

        UserForListDtos = Mapper.Map<List<UserForListDto>>(Users);
        UserForDetailDtos = Mapper.Map<List<UserForDetailDto>>(Users);
    }
}