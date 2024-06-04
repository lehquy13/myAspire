using LibraryManagement.Application.Contracts.Baskets;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Application.Contracts.Interfaces;

public interface IBasketServices
{
    Task<Result<BasketForDetailDto>> GetBasketAsync(Guid userId);
    
    Task<Result> AddItemToBasket(AddItemToBasketCommand addItemToBasketCommand);
    
    Task<Result> DeleteBasketAsync(int basketId);
    
    Task<Result> SetQuantities(SetQuantitiesCommand setQuantitiesCommand);
}