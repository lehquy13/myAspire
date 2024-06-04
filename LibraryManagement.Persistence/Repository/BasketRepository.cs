using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Persistence.Entity_Framework_Core;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repository;

public class BasketRepository : RepositoryImpl<Basket, int>, IBasketRepository
{
    public BasketRepository(AppDbContext appDbContext, IAppLogger<BasketRepository> logger) : base(appDbContext, logger)
    {
    }


    public async Task<Basket?> GetBasketByUserIdAsync(IdentityGuid userId)
    {
        try
        {
            return await AppDbContext.Baskets
                .Include(b => b.Items)
                .ThenInclude(bi => bi.Book)
                .ThenInclude(bo => bo.Authors)
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message, ex);
            return null;
        }
    }

    public override async Task<Basket?> GetByIdAsync(int id)
    {
        try
        {
            return await AppDbContext.Baskets
                .Include(b => b.Items)
                .ThenInclude(bi => bi.Book)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message, ex);
            return null;
        }
    }
}