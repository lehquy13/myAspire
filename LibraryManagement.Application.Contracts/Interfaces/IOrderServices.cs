using LibraryManagement.Application.Contracts.Order;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Application.Contracts.Interfaces;

public interface IOrderServices
{
    Task<PaginationResult<OrderForDetailDto>> GetAllOrders(OrderPaginatedParams paginatedParams);
    
    Task<Result<OrderForDetailDto>> CreateOrder(Guid userId);
    
    Task<Result> PurchaseOrder(Guid customerId, Guid orderId, string paymentMethod);
    Task<Result<OrderForDetailDto>> GetOrder(Guid guid);
}