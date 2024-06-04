using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Domain.DomainServices.Interfaces;

public interface IBasketDomainServices
{
    Task<Result> AddItemToBasket(IdentityGuid id, int bookId, decimal price, int quantity = 1);

    Task DeleteBasketAsync(int basketId);

    Task<Result<Basket>> SetQuantities(IdentityGuid identityGuid, Dictionary<int, int> quantities);
}