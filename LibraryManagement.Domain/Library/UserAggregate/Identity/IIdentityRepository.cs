using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;

namespace LibraryManagement.Domain.Library.UserAggregate.Identity;

public interface IIdentityRepository : IRepository<IdentityUser, IdentityGuid>
{
    Task<IdentityUser?> FindByEmailAsync(string email);
}