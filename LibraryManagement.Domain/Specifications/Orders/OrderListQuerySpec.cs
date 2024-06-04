using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;

namespace LibraryManagement.Domain.Specifications.Orders;

public sealed class OrderListQuerySpec : GetListSpecificationBase<Order>
{
    public OrderListQuerySpec(int pageIndex, int pageSize, IdentityGuid? userId)
        : base(pageIndex, pageSize)
    {
        Criteria = ord => 
            (userId == null || ord.UserId == userId);
        
        AddInclude(ord => ord.User);
        AddInclude(ord => ord.OrderItems);
        AddInclude("OrderItems.Book");
    }
}