using LibraryManagement.Domain.DomainServices.Exceptions;
using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.OrderAggregate;
using LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Shared.Enums;
using LibraryManagement.Domain.Shared.Results;
using LibraryManagement.Domain.Specifications.Books;
using LibraryManagement.Domain.Specifications.Orders;

namespace LibraryManagement.Domain.DomainServices;

public class OrderDomainServices : IOrderDomainServices
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IIdentityRepository _identityRepository;
    private readonly IAppLogger<OrderDomainServices> _logger;

    public OrderDomainServices(
        IBasketRepository basketRepository,
        IBookRepository bookRepository,
        IOrderRepository orderRepository,
        IAppLogger<OrderDomainServices> logger,
        IIdentityRepository identityRepository)
    {
        _orderRepository = orderRepository;
        _logger = logger;
        _identityRepository = identityRepository;
        _basketRepository = basketRepository;
        _bookRepository = bookRepository;
    }

    public async Task<Result<Order>> CreateOrderAsync(IdentityGuid customerId)
    {
        var basket = await _basketRepository.GetBasketByUserIdAsync(customerId);

        if (basket is null)
        {
            _logger.LogError(DomainServiceErrors.BasketNotFound);
            return Result.Fail(DomainServiceErrors.BasketNotFound);
        }

        if (basket.Items.Count <= 0)
        {
            _logger.LogError(DomainServiceErrors.BasketNotFound);
            return Result.Fail(DomainServiceErrors.BasketIsEmpty);
        }

        var itemsSpecification = basket.Items.Select(item => item.BookId).ToList();
        var bookSpec = await _bookRepository.GetAllListAsync(
            new BookListByIdQuerySpec(itemsSpecification));

        var order = new Order(basket.UserId);

        var items = basket.Items.Select(basketItem =>
        {
            var book = bookSpec.First(c => c.Id == basketItem.BookId);
            var orderItem = new OrderItem(book.Id, basketItem.Quantity, book.CurrentPrice);

            return orderItem;
        }).ToArray();

        order.AddOrderItems(items);

        await _orderRepository.InsertAsync(order);

        //Remove items from basket
        //1. Consider it to be a domain event
        //2. It is too simple / explicit to be a domain event
        basket.ClearItems();

        return order;
    }

    public async Task<Result<Order>> PurchaseOrder(IdentityGuid customerId, OrderGuid orderId,
        PaymentMethod paymentMethod)
    {
        var customerOrder = await _orderRepository.GetByIdAsync(
            new OrderBelongToUserSpec(orderId, customerId)
        );

        if (customerOrder is null)
        {
            _logger.LogError(DomainServiceErrors.OrderNotFound);
            return Result.Fail(DomainServiceErrors.OrderNotFound);
        }

        if (customerOrder.OrderStatus == OrderStatus.Completed)
        {
            _logger.LogError(DomainServiceErrors.OrderAlreadyCompleted);
            return Result.Fail(DomainServiceErrors.OrderAlreadyCompleted);
        }

        var customer = await _identityRepository.GetByIdAsync(customerId);

        if (customer is null)
        {
            _logger.LogError(DomainServiceErrors.UserNotFound);
            return Result.Fail(DomainServiceErrors.UserNotFound);
        }

        //Handle does all items are available
        bool isAllItemsAvailable = true;
        foreach (var item in customerOrder.OrderItems)
        {
            if (item.Quantity > item.Book.Quantity)
            {
                customerOrder.UpdateOrderStatus(OrderStatus.Unavailable);

                _logger.LogError("{message}{orderId}", DomainServiceErrors.InsufficientBookQuantityWithSpecificOrderId);

                isAllItemsAvailable = false;
            }
        }

        if (!isAllItemsAvailable)
        {
            return Result.Fail(DomainServiceErrors.InsufficientBookQuantity);
        }

        //Check does user balance is enough
        if (customerOrder.TotalPrice > customer.BalanceAmount)
        {
            _logger.LogError("{message} while purchasing order with Id: {orderId}",
                DomainServiceErrors.InsufficientUserBalance, orderId.Value);

            return Result.Fail(DomainServiceErrors.InsufficientUserBalance);
        }

        customer.Purchase(customerOrder.TotalPrice);

        customerOrder.PurchaseOrder(paymentMethod);

        return customerOrder;
    }
}