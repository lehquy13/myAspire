using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Specifications.Orders;
using LibraryManagement.Persistence.Entity_Framework_Core;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repository;

public class OrderRepository : RepositoryImpl<Order, OrderGuid>, IOrderRepository
{
    public OrderRepository(AppDbContext appDbContext, IAppLogger<OrderRepository> logger) : base(appDbContext, logger)
    {
    }

    public async Task<List<Order>> GetAllListAsync(OrderListQuerySpec orderListQuerySpec)
    {
        try
        {
            var result = GetQuery(AppDbContext.Set<Order>(), orderListQuerySpec);

            return await result.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
            return new();
        }
    }
}