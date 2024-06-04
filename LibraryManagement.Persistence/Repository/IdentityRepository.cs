using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Persistence.Entity_Framework_Core;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repository;

public class IdentityRepository : RepositoryImpl<IdentityUser, IdentityGuid>, IIdentityRepository
{
    public IdentityRepository(AppDbContext appDbContext, IAppLogger<IdentityRepository> logger) : base(appDbContext,
        logger)
    {
    }

    public async Task<IdentityUser?> FindByEmailAsync(string email)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .Include(x => x.User)
                .Include(x => x.IdentityRole)
                .FirstOrDefaultAsync(x => x.User.Email == email);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "FindByEmailAsync", ex.Message);
            return null;
        }
    }

    public override async Task<IdentityUser?> GetByIdAsync(IdentityGuid id)
    {
        try
        {
            return await AppDbContext.IdentityUsers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetByIdAsync", ex.Message);
            return null;
        }
    }
}