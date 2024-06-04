using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Application.Mapping;
using LibraryManagement.Application.ServiceImpls;
using LibraryManagement.Domain.DomainServices;
using LibraryManagement.Domain.DomainServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddApplicationMappings();
            
            //Domain services
            services.AddTransient<IBasketDomainServices, BasketDomainServices>();
            services.AddTransient<IIdentityDomainServices, IdentityDomainServices>();
            services.AddTransient<IOrderDomainServices, OrderDomainServices>();
            services.AddTransient<IBookDomainServices, BookDomainServices>();
            services.AddTransient<IWishListDomainServices, WishListDomainServices>();

            //Application services
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IBasketServices, BasketServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IBookServices, BookServices>();
            services.AddScoped<IWishListServices, WishListServices>();

            return services;
        }
    }
}