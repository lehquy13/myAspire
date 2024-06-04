using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Domain.DomainServices.Interfaces;

public interface IOrderDomainServices
{
    Task<Result<Order>> CreateOrderAsync(IdentityGuid customerId);
    
    Task<Result<Order>> PurchaseOrder(IdentityGuid customerId, OrderGuid orderId, PaymentMethod paymentMethod);
}