using LibraryManagement.Domain.DomainServices.Exceptions;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Domain.DomainServices;

public class BasketDomainServices : IBasketDomainServices
{
    private readonly IBasketRepository _basketRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IAppLogger<BasketDomainServices> _logger;

    public BasketDomainServices(IBasketRepository basketRepository,
        IAppLogger<BasketDomainServices> logger, IBookRepository bookRepository)
    {
        _basketRepository = basketRepository;
        _logger = logger;
        _bookRepository = bookRepository;
    }

    public async Task<Result> AddItemToBasket(IdentityGuid id, int bookId, decimal price, int quantity = 1)
    {
        var basket = await _basketRepository.GetBasketByUserIdAsync(id);

        if (basket is null)
        {
            basket = new Basket(id);
            await _basketRepository.InsertAsync(basket);
        }

        var book = await _bookRepository.GetByIdAsync(bookId);

        if (book is null)
        {
            //throw new BookNotFoundException(bookId);
            _logger.LogError(DomainServiceErrors.BookNotFound);
            return Result.Fail(DomainServiceErrors.BookNotFound);
        }

        basket.AddItem(book.Id, quantity);

        return Result.Success();
    }

    public async Task DeleteBasketAsync(int basketId)
    {
        await _basketRepository.DeleteByIdAsync(basketId);
    }

    public async Task<Result<Basket>> SetQuantities(IdentityGuid identityGuid, Dictionary<int, int> quantities)
    {
        var basket = await _basketRepository.GetBasketByUserIdAsync(identityGuid);

        if (basket == null)
        {
            _logger.LogError(DomainServiceErrors.BasketNotFound);
            return Result.Fail(DomainServiceErrors.BasketNotFound);
        }

        foreach (var item in basket.Items)
        {
            if (quantities.TryGetValue(item.BookId, out var quantity))
            {
                _logger.LogInformation($"Updating quantity of item ID:{item.Id} to {quantity}.");
                item.Quantity = quantity;

                quantities.Remove(item.BookId);
            }
        }

        if (quantities.Count > 0)
        {
            _logger.LogError(DomainServiceErrors.BasketItemNotFound);
            return Result.Fail(DomainServiceErrors.BasketItemNotFound);
        }

        basket.RemoveEmptyItems();

        return basket;
    }
}