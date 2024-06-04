using LibraryManagement.Application.Contracts.Order;
using LibraryManagement.Domain.Library.OrderAggregate;
using Mapster;

namespace LibraryManagement.Application.Mapping;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderItem, OrderItemForListDto>()
            .Map(dest => dest.Title, src => src.Book.Title)
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest, src => src);

        config.NewConfig<Order, OrderForDetailDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod.ToString())
            .Map(dest => dest.OrderStatus, src => src.OrderStatus.ToString())
            .Map(dest => dest.TotalPrice, src => src.TotalPrice)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
            .Map(dest => dest, src => src);
    }
}