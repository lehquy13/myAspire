using LibraryManagement.Domain.Library.OrderAggregate;

namespace LibraryManagement.Domain.Specifications.Orders;

public class OrderFilterByDateSpec : SpecificationBase<Order>
{
    public OrderFilterByDateSpec(DateTime? fromDate, DateTime? toDate)
    {
        Criteria = order =>
            (fromDate == null || order.CreatedAt >= fromDate) &&
            (toDate == null || order.CreatedAt <= toDate);
        
        AddInclude(order => order.OrderItems);
    }
}