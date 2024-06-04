using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;

namespace LibraryManagement.Domain.Library.UserAggregate;

public interface IUserRepository : IRepository<User, IdentityGuid>
{
    Task<User?> GetFullById(IdentityGuid id);
}