using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;

namespace LibraryManagement.Domain.Library.BasketAggregate;

public interface IBasketRepository : IRepository<Basket, int>
{
    Task<Basket?> GetBasketByUserIdAsync(IdentityGuid userId);
}