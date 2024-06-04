using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Domain.DomainServices.Interfaces;

public interface IIdentityDomainServices
{
    Task<IdentityUser?> SignInAsync(string email, string password);

    Task<IdentityUser?> FindByEmailAsync(string email);
    
    Task<IdentityUser?> GetUserIdAsync(IdentityGuid id);

    Task<Result<IdentityUser>> CreateAsync(
        string email,
        string password,
        string phoneNumber,
        string name,
        string city,
        string country);

    Task<Result> ChangePassword(IdentityGuid identityId, string currentPassword, string newPassword);
}