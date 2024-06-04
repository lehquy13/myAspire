using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Persistence.Entity_Framework_Core;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repository;

public class UserRepository : RepositoryImpl<User, IdentityGuid>, IUserRepository
{
    public UserRepository(AppDbContext appDbContext, IAppLogger<UserRepository> logger) : base(appDbContext, logger)
    {
    }

    public async Task<User?> GetFullById(IdentityGuid id)
    {
        try
        {
            var fullUser = await AppDbContext
                .Users
                .Include(x => x.FavouriteBooks)
                .ThenInclude(x => x.Authors)
                .Include(x => x.WishLists)
                .ThenInclude(x => x.Authors)
                .FirstOrDefaultAsync(x => x.Id == id);
            return fullUser;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetFullById", ex.Message);
            return null;
        }
    }
}