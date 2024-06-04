using LibraryManagement.Application.Contracts.Authentications;
using LibraryManagement.Application.Contracts.Users;
using LibraryManagement.Domain.Library.UserAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using Mapster;

namespace LibraryManagement.Application.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserForLoginResponseDto>()
            .Map(des => des.Name, src => src.Name)
            .Map(des => des, src => src); 
        
        config.NewConfig<IdentityUser, UserForLoginResponseDto>()
            .Map(des => des.Name, src => src.User.Name)
            .Map(des => des.Email, src => src.User.Email)
            .Map(des => des.Role, src => src.IdentityRole.Name)
            .Map(des => des.Id, src => src.Id.Value);
        
        

        config.NewConfig<User, UserForListDto>()
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des.Name, src => src.Name)
            .Map(des => des, src => src);

        config.NewConfig<User, UserForDetailDto>()
            .Map(des => des.BalanceAmount, src => src.IdentityUser.BalanceAmount)
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.PhoneNumber, src => src.IdentityUser.PhoneNumber)
            .Map(des => des.Address, src => src.Address.ToString())
            .Map(des => des.Name, src => src.Name)
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);

        config.NewConfig<User, UserForBasicDto>()
            .Map(des => des.WishList, src => src.WishLists)
            .Map(des => des.FavouriteBooks, src => src.FavouriteBooks)
            .Map(des => des.Email, src => src.Email)
            .Map(des => des.Address, src => src.Address.ToString())
            .Map(des => des.Name, src => src.Name)
            .Map(des => des.Id, src => src.Id.Value)
            .Map(des => des, src => src);
        
        config.NewConfig<UserForUpsertDto, User>()
            .Map(des => des.Address, src => new Address(src.City, src.Country))
            .Map(des => des, src => src);

    }
}