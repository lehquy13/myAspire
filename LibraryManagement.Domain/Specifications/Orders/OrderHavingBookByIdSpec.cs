using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Enums;

namespace LibraryManagement.Domain.Specifications.Orders;

public class OrderHavingBookByIdSpec : SpecificationBase<Order>
{
    public OrderHavingBookByIdSpec(int bookId, IdentityGuid userId)
    {
        Criteria = order =>
            order.OrderItems.Any(x => x.BookId == bookId) &&
            order.UserId == userId &&
            order.OrderStatus == OrderStatus.Completed;
    }
}