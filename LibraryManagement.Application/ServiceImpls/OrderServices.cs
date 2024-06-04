using LibraryManagement.Application.Contracts.Commons.ErrorMessages;
using LibraryManagement.Application.Contracts.Interfaces;
using LibraryManagement.Application.Contracts.Order;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Results;
using LibraryManagement.Domain.Shared.Utilities;
using LibraryManagement.Domain.Specifications.Orders;
using MapsterMapper;

namespace LibraryManagement.Application.ServiceImpls;

public class OrderServices : ServiceBase, IOrderServices
{
    private readonly IOrderDomainServices _orderDomainServices;
    private readonly IEmailSender _emailSender;
    private readonly IRepository<Order, OrderGuid> _orderRepository;

    public OrderServices(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IAppLogger<OrderServices> logger,
        IOrderDomainServices orderDomainServices,
        IRepository<Order, OrderGuid> orderRepository, IEmailSender emailSender)
        : base(mapper, unitOfWork, logger)
    {
        _orderDomainServices = orderDomainServices;
        _orderRepository = orderRepository;
        _emailSender = emailSender;
    }

    public async Task<PaginationResult<OrderForDetailDto>> GetAllOrders(OrderPaginatedParams paginatedParams)
    {
        var orderSpec = new OrderListQuerySpec(
            paginatedParams.PageIndex,
            paginatedParams.PageSize,
            paginatedParams.UserId != Guid.Empty ? IdentityGuid.Create(paginatedParams.UserId) : null);

        var totalItems = await _orderRepository.CountAsync(orderSpec);

        var orders = await _orderRepository.GetAllListAsync(orderSpec);

        var orderForDetailDtos = Mapper.Map<List<OrderForDetailDto>>(orders);

        return PaginationResult<OrderForDetailDto>
            .Success(
                orderForDetailDtos,
                totalItems,
                paginatedParams.PageIndex,
                paginatedParams.PageSize);
    }

    public async Task<Result<OrderForDetailDto>> CreateOrder(Guid userId)
    {
        try
        {
            var result = await _orderDomainServices.CreateOrderAsync(IdentityGuid.Create(userId));

            if (!result.IsSuccess)
            {
                var failResultToReturn = Result
                    .Fail(OrderErrorMessages.CreateOrderFail)
                    .WithError(result.DisplayMessage);
                return failResultToReturn;
            }

            if (await UnitOfWork.SaveChangesAsync() <= 0)
            {
                return Result.Fail(OrderErrorMessages.CreateOrderFailWhileSavingChanges);
            }

            var orderForDetailDto = Mapper.Map<OrderForDetailDto>(result.Value);

            return orderForDetailDto;
        }
        catch (Exception e)
        {
            Logger.LogError($"{OrderErrorMessages.CreateOrderFailWithException} {e.Message}");
            var failResultToReturn = Result
                .Fail(OrderErrorMessages.CreateOrderFailWithException)
                .WithError(e.Message);
            
            return failResultToReturn;
        }
    }

    public async Task<Result> PurchaseOrder(Guid customerId, Guid orderId, string paymentMethod)
    {
        var result = await _orderDomainServices
            .PurchaseOrder(
                IdentityGuid.Create(customerId),
                OrderGuid.Create(orderId),
                paymentMethod.ToEnum<PaymentMethod>()
            );

        if (!result.IsSuccess)
        {
            return Result.Fail(result.DisplayMessage);
        }

        if (await UnitOfWork.SaveChangesAsync() <= 0)
        {
            return Result.Fail(OrderErrorMessages.PurchaseOrderFailWhileSavingChanges);
        }

        await _emailSender.SendEmail(
            "hoangle.q3@gmail.com", // This will be replace by customer Email, then its logic will be right
            $"You have just purchased an order {orderId} in {DateTime.Now:dd-MM-yyyy}",
            EmailContentGenerate(DateTime.Now, result.Value.PaymentMethod.ToString(), result.Value.TotalPrice));

        return Result.Success();
    }

    private string EmailContentGenerate(DateTime dateTime, string paymentMethod, decimal totalPrice)
    {
        return
            $"Your order has been purchased successfully at {dateTime} with payment method {paymentMethod} and total price {totalPrice}";
    }

    public async Task<Result<OrderForDetailDto>> GetOrder(Guid guid)
    {
        var orderSpec = await _orderRepository.GetByIdAsync(OrderGuid.Create(guid));

        if (orderSpec is null)
        {
            return Result.Fail(OrderErrorMessages.OrderNotFound);
        }

        var orderForDetailDto = Mapper.Map<OrderForDetailDto>(orderSpec);

        return Result<OrderForDetailDto>.Success(orderForDetailDto);
    }
}