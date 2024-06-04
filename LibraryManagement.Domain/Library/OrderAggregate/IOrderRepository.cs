using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Specifications.Orders;

namespace LibraryManagement.Domain.Library.OrderAggregate;

public interface IOrderRepository : IRepository<Order,OrderGuid>
{
    Task<List<Order>> GetAllListAsync(OrderListQuerySpec orderListQuerySpec);
}