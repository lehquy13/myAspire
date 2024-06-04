using LibraryManagement.Domain.Shared.Paginations;

namespace LibraryManagement.Application.Contracts.Order;

public class OrderPaginatedParams : PaginatedParams
{
   public Guid UserId { get; set; } = Guid.Empty;
}