using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;

namespace LibraryManagement.Domain.Specifications.Orders;

public class OrderBelongToUserSpec : SpecificationBase<Order>
{
    public OrderBelongToUserSpec(OrderGuid orderId, IdentityGuid userId)
    {
        Criteria = order => 
            order.Id == orderId && order.UserId == userId;
        
        AddInclude(ord => ord.User);
        AddInclude(ord => ord.OrderItems);
        AddInclude("OrderItems.Book");
    }
}