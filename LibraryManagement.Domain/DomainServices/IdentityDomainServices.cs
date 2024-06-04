using LibraryManagement.Domain.DomainServices.Exceptions;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Domain.DomainServices;

public class IdentityDomainServices : IIdentityDomainServices
{
    private readonly IIdentityRepository _identityUserRepository;
    private readonly IRepository<IdentityRole, int> _identityRoleRepository;
    private readonly IAppLogger<IdentityDomainServices> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Basket, int> _basketRepository;

    public IdentityDomainServices(
        IIdentityRepository identityUserRepository,
        IRepository<IdentityRole, int> identityRoleRepository,
        IRepository<Basket, int> basketRepository,
        IAppLogger<IdentityDomainServices> logger,
        IUnitOfWork unitOfWork)
    {
        _identityUserRepository = identityUserRepository;
        _identityRoleRepository = identityRoleRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _basketRepository = basketRepository;
    }

    public async Task<IdentityUser?> SignInAsync(string email, string password)
    {
        await Task.CompletedTask;

        var identityUser = await _identityUserRepository
            .FindByEmailAsync(email);

        if (identityUser is null || identityUser.ValidatePassword(password) is false)
        {
            return null;
        }

        return identityUser;
    }

    public async Task<IdentityUser?> FindByEmailAsync(string email)
    {
        var identityUser = await _identityUserRepository
            .FindByEmailAsync(email);

        return identityUser;
    }

    public async Task<IdentityUser?> GetUserIdAsync(IdentityGuid id)
    {
        var identityUser = await _identityUserRepository
            .GetByIdAsync(id);

        return identityUser;
    }

    public async Task<Result<IdentityUser>> CreateAsync(
        string email,
        string password,
        string phoneNumber,
        string name,
        string city,
        string country)
    {
        var role = await _identityRoleRepository.GetByIdAsync(1); // User role Id = 2, better change this to be a constant

        if (role is null)
        {
            return Result.Fail(DomainServiceErrors.RoleNotFound);
        }

        User user = new(email, name, new Address() { City = city, Country = country });
        IdentityUser identityUser = new IdentityUser(phoneNumber, password);

        identityUser.WithUser(user);
        identityUser.WithRole(role);

        if (!await _identityUserRepository.InsertAsync(identityUser))
        {
            _logger.LogError(DomainServiceErrors.InsertUserFail);
            return Result.Fail(DomainServiceErrors.InsertUserFail);
        }

        Basket basket = new(identityUser.Id);

        if (!await _basketRepository.InsertAsync(basket))
        {
            _logger.LogError(DomainServiceErrors.InsertUserBasketFail);
            return Result.Fail(DomainServiceErrors.InsertUserBasketFail);
        }

        return identityUser;
    }

    public async Task<Result> ChangePassword(IdentityGuid identityId, string currentPassword, string newPassword)
    {
        var identityUser = await _identityUserRepository.GetByIdAsync(identityId);

        if (identityUser is null)
        {
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }

        var verifyResult = identityUser.ValidatePassword(currentPassword);

        if (!verifyResult)
        {
            return Result.Fail(DomainServiceErrors.InvalidPassword);
        }

        identityUser.ChangePassword(newPassword);

        return Result.Success();
    }
}