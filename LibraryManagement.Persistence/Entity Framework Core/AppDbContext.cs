using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryManagement.Persistence.Entity_Framework_Core;

public class AppDbContext : DbContext
{
    //BasketAggregates
    public DbSet<Basket> Baskets { get; set; } = null!;

    public DbSet<BasketItem> BasketItems { get; set; } = null!;

    //BookAggregates
    public DbSet<Book> Books { get; set; } = null!;

    public DbSet<Author> Authors { get; set; } = null!;

    public DbSet<Review> Reviews { get; set; } = null!;

    //OrderAggregates
    public DbSet<Order> Orders { get; set; } = null!;

    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    //UserAggregates
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<IdentityUser> IdentityUsers { get; set; } = null!;

    public DbSet<IdentityRole> IdentityRoles { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

//using to support add migration design time
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite("Data Source=LibraryQuyLH5.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }