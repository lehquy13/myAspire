using LibraryManagement.Application.Contracts.Baskets;
using LibraryManagement.Domain.Library.BasketAggregate;
using Mapster;

namespace LibraryManagement.Application.Mapping;

public class BasketMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BasketItem, BasketItemForList>()
            .Map(dest => dest.BookForListDto, src => src.Book)
            .Map(dest => dest, src => src);

        config.NewConfig<Basket, BasketForDetailDto>()
            .Map(dest => dest.TotalPrice, src => src.Items.Sum(x => x.Book.CurrentPrice))
            .Map(dest => dest, src => src);
    }
}