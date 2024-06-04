using LibraryManagement.Application.Contracts.Commons.ErrorMessages;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Application.Contracts.Users;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Paginations;
using LibraryManagement.Domain.Shared.Results;
using LibraryManagement.Domain.Specifications.Users;
using MapsterMapper;

namespace LibraryManagement.Application.ServiceImpls;

public class UserServices : ServiceBase, IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityDomainServices _identityDomainServices;

    public UserServices(
        IMapper mapper,
        IAppLogger<UserServices> logger,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository, IIdentityDomainServices identityDomainServices) : base(mapper, unitOfWork,
        logger)
    {
        _userRepository = userRepository;
        _identityDomainServices = identityDomainServices;
    }

    public async Task<PaginationResult<UserForListDto>> GetUsers(PaginatedParams userFilterParams)
    {
        var userSpec = new UserListQuerySpec(userFilterParams.PageIndex, userFilterParams.PageSize);

        var totalCount = await _userRepository.CountAsync(userSpec);

        var users = await _userRepository.GetAllListAsync(userSpec);
        var usersForListDto = Mapper.Map<List<UserForListDto>>(users);

        return PaginationResult<UserForListDto>
            .Success(
                usersForListDto,
                totalCount,
                userFilterParams.PageIndex,
                userFilterParams.PageSize);
    }

    public async Task<Result<UserForDetailDto>> GetUserDetailByIdAsync(Guid id)
    {
        try
        {
            var user = await _identityDomainServices.GetUserIdAsync(IdentityGuid.Create(id));

            if (user is null)
            {
                Logger.LogError("{Message} with Id {id}", UserErrorMessages.UserNotFound, id);
                return Result.Fail(UserErrorMessages.UserNotFound);
            }

            var userForDetailDto = Mapper.Map<UserForDetailDto>(user.User);

            return userForDetailDto;
        }
        catch (Exception e)
        {
            Logger.LogError(e.Message);
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result<UserForBasicDto>> GetUserBasicByIdAsync(Guid id)
    {
        var user = await _userRepository.GetFullById(IdentityGuid.Create(id));

        if (user is null)
        {
            Logger.LogError("{Message} with Id {id}", UserErrorMessages.UserNotFound, id);
            return Result.Fail(UserErrorMessages.UserNotFound);
        }

        var userForDetailDto = Mapper.Map<UserForBasicDto>(user);

        return userForDetailDto;
    }

    public async Task<Result> UpsertUserAsync(UserForUpsertDto userForUpsertDto)
    {
        var user = await _userRepository.GetByIdAsync(IdentityGuid.Create(userForUpsertDto.Id));

        if (user is null)
        {
            //Create new user
            user = Mapper.Map<User>(userForUpsertDto);

            await _userRepository.InsertAsync(user);
        }
        else
        {
            //Update user
            Mapper.Map(userForUpsertDto, user);
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            Logger.LogError("{Message}", UserErrorMessages.UpsertFailWithException);
            return Result.Fail(UserErrorMessages.UpsertFailWithException);
        }

        return Result.Success();
    }

    public async Task<Result> DeleteUserAsync(Guid id)
    {
        var userDeleteResult = await _userRepository.DeleteByIdAsync(IdentityGuid.Create(id));

        if (userDeleteResult is false)
        {
            Logger.LogError("{Message} with Id {id}", UserErrorMessages.UserNotFound, id);
            return Result.Fail(UserErrorMessages.UserNotFound);
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            Logger.LogError("{Message}", UserErrorMessages.DeleteFailWithException);
            return Result.Fail(UserErrorMessages.DeleteFailWithException);
        }

        return Result.Success();
    }

    public async Task<Result<decimal>> DepositAsync(Guid id, decimal amount)
    {
        var user = await _identityDomainServices.GetUserIdAsync(IdentityGuid.Create(id));

        if (user is null)
        {
            Logger.LogError("{Message} with Id {id}", UserErrorMessages.UserNotFound, id);
            return Result.Fail(UserErrorMessages.UserNotFound);
        }

        user.Deposit(amount);

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            Logger.LogError("{Message}", UserErrorMessages.DepositFailWhileSavingChanges);
            return Result.Fail(UserErrorMessages.DepositFailWhileSavingChanges);
        }

        return Result<decimal>.Success(amount);
    }
}