using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Persistence.Entity_Framework_Core;
using LibraryManagement.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            ConfigurationManager configuration
        )
        {
            // set configuration settings to emailSettingName and turn it into Singleton

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection")
                )
            );

            //Seed data using DataSeed
            var dbContext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
            DataSeed.Execute(dbContext).GetAwaiter();

            // Dependency Injection for repository
            services.AddLazyCache();

            services.AddScoped(typeof(IRepository<,>), typeof(RepositoryImpl<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

   
          
            //services.AddScoped(typeof(IBookRepository), typeof(CachedBookRepositoryImpl));
            services.AddScoped(typeof(IBookRepository), typeof(BookRepository));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();

            return services;
        }
    }
}