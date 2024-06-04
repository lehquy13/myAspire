using System.Reflection;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Shared.Enums;
using Newtonsoft.Json;

namespace LibraryManagement.Persistence.Entity_Framework_Core;

public static class DataSeed
{
    public static async Task Execute(AppDbContext context)
    {
        try
        {
            Console.WriteLine("Checking database is created or not...");
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("Checked!");

            // Look for any Books.
            if (!context.Books.Any())
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                //Handle data seed for author
                var dataFile = await File.ReadAllTextAsync(path + "/DataFiles/authorsData.json");

                var authors = JsonConvert.DeserializeObject<List<Author>>(dataFile) ?? new List<Author>();
                context.Authors.AddRange(authors);

                //Handle data seed for books
                dataFile = await File.ReadAllTextAsync(path + "/DataFiles/booksData.json");

                var books = JsonConvert.DeserializeObject<List<Book>>(dataFile) ?? new List<Book>();

                foreach (var book in books)
                {
                    Random random = new();

                    var count = random.Next(1, 3);
                    var authorsToAdd = new List<Author>();
                    for (int i = 0; i < count; i++)
                    {
                        var iAuthor = random.Next(0, authors.Count);
                        authorsToAdd.Add(authors[iAuthor]);
                    }

                    book.Authors = (authorsToAdd);

                    Array values = Enum.GetValues(typeof(Genre));
                    Genre genre = (Genre)(values.GetValue(random.Next(values.Length)) ?? Genre.Adventure);

                    book.Genre = genre;
                }

                context.Books.AddRange(books);

                //Handle data seed for customers
                var identityRoles = new List<IdentityRole>()
                {
                    new()
                    {
                        Name = "Admin"
                    },
                    new()
                    {
                        Name = "User"
                    }
                };

                context.IdentityRoles.AddRange(identityRoles);

                dataFile = await File.ReadAllTextAsync(path + "/DataFiles/customersData.json");

                var customers = JsonConvert.DeserializeObject<List<User>>(dataFile) ?? new List<User>();

                //Handle favorite books data seed
                foreach (var cus in customers)
                {
                    Random rnd = new Random();
                    cus.AddFavoriteBook(books.Skip(rnd.Next(1, 10)).Take(rnd.Next(3, 5)).ToList());
                    cus.AddToWishList(books.Skip(rnd.Next(1, 10)).Take(rnd.Next(3, 5)).ToList());
                }

                dataFile = await File.ReadAllTextAsync(path + "/DataFiles/identityUsersData.json");

                var identityUsers = JsonConvert
                    .DeserializeObject<List<IdentityUser>>(dataFile) ?? new List<IdentityUser>();

                for (int i = 0; i < customers.Count; i++)
                {
                    identityUsers[i].WithRole(identityRoles[1]);
                    identityUsers[i].WithUser(customers[i]);
                    identityUsers[i].Deposit((new Random()).Next(1000, 100000));
                }

                identityUsers.Last().WithRole(identityRoles[0]); // admin account

                context.IdentityUsers.AddRange(identityUsers);

                //Handle Basket data seed
                var baskets = new List<Basket>();

                foreach (var t in customers)
                {
                    var basket = new Basket(t.Id);

                    Random random = new();
                    for (int j = 0; j < random.Next(1, 3); j++)
                    {
                        var iBook = random.Next(1, books.Count);
                        basket.AddItem(iBook, random.Next(1, 3));
                    }

                    baskets.Add(basket);
                }

                context.Baskets.AddRange(baskets);

                //Handle reviews data seed
                dataFile = await File.ReadAllTextAsync(path + "/DataFiles/reviewsData.json");

                var sampleReviews = JsonConvert.DeserializeObject<List<Review>>(dataFile) ?? new List<Review>();
                foreach (var review in sampleReviews)
                {
                    Random random = new();
                    var iCustomer = random.Next(0, 10);
                    var reviewToAdd = new Review(review.Title, review.Content, review.Rating, review.Image,
                        review.IsLike, customers[iCustomer].Id);

                    books[random.Next(0, 10)].AddReview(reviewToAdd);
                }

                //context.Reviews.AddRange(reviewsToAdd);

                await context.SaveChangesAsync();

                Console.WriteLine("All done. Enjoy!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}