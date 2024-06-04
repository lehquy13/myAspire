using LibraryManagement.Application.Contracts.Baskets;
using LibraryManagement.Application.Contracts.Commons.ErrorMessages;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;
using MapsterMapper;

namespace LibraryManagement.Application.ServiceImpls;

public class BasketServices : ServiceBase, IBasketServices
{
    private readonly IBasketDomainServices _basketDomainServices;
    private readonly IBasketRepository _basketRepository;

    public BasketServices(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IAppLogger<ServiceBase> logger,
        IBasketDomainServices basketDomainServices,
        IBasketRepository basketRepository)
        : base(mapper, unitOfWork, logger)
    {
        _basketDomainServices = basketDomainServices;
        _basketRepository = basketRepository;
    }

    public async Task<Result<BasketForDetailDto>> GetBasketAsync(Guid userId)
    {
        try
        {
            var result = await _basketRepository.GetBasketByUserIdAsync(IdentityGuid.Create(userId));

            if (result is null)
            {
                return Result.Fail(BasketErrorMessages.BasketNotFound);
            }

            var basketForDetailDto = Mapper.Map<BasketForDetailDto>(result);

            return Result<BasketForDetailDto>.Success(basketForDetailDto);
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result> AddItemToBasket(AddItemToBasketCommand addItemToBasketCommand)
    {
        var result =
            await _basketDomainServices.AddItemToBasket(
                IdentityGuid.Create(addItemToBasketCommand.UserId),
                addItemToBasketCommand.BookId,
                addItemToBasketCommand.Price,
                addItemToBasketCommand.Quantity);

        if (!result.IsSuccess)
        {
            Logger.LogError(result.DisplayMessage);

            var resultToReturn = Result.Fail(BasketErrorMessages.AddItemFail);
            resultToReturn.ErrorMessages.Add(result.DisplayMessage);
            return resultToReturn;
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            //throw new AddItemFailWhileSavingChangesException();
            Logger.LogError(BasketErrorMessages.AddItemFailWhileSavingChanges);
            return Result.Fail(BasketErrorMessages.AddItemFailWhileSavingChanges);
        }

        return Result.Success();
    }

    public async Task<Result> DeleteBasketAsync(int basketId)
    {
        await _basketDomainServices.DeleteBasketAsync(basketId);

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            //throw new AddItemFailWhileSavingChangesException();
            Logger.LogError(BasketErrorMessages.AddItemFailWhileSavingChanges);
            return Result.Fail(BasketErrorMessages.AddItemFailWhileSavingChanges);
        }

        return Result.Success();
    }

    public async Task<Result> SetQuantities(SetQuantitiesCommand setQuantitiesCommand)
    {
        var result =
            await _basketDomainServices.SetQuantities(
                IdentityGuid.Create(setQuantitiesCommand.UserId),
                setQuantitiesCommand.Quantities);

        if (!result.IsSuccess)
        {
            return Result
                .Fail(BasketErrorMessages.SetQuantitiesFail)
                .WithError(result.DisplayMessage);
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(BasketErrorMessages.SetQuantitiesFailWhileSavingChanges);
        }

        return Result.Success();
    }
}