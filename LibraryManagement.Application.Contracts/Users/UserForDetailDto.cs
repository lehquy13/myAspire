using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.Application.Contracts.Commons.Primitives;
using LibraryManagement.Application.Contracts.Interfaces;

namespace LibraryManagement.Application.Contracts.Users;

public class UserForDetailDto : EntityDto<Guid>, IAuditableEntityDto
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public decimal BalanceAmount { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return $"User with Id: {Id}" +
               $"\nName: {Name}" +
               $"\nBalance: {BalanceAmount}" +
               $"\nEmail: {Email}" +
               $"\nPhoneNumber: {PhoneNumber}" +
               $"\nAddress: {Address}";
    }
}

public class UserForBasicDto : EntityDto<Guid>, IAuditableEntityDto
{
    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<BookForListDto> WishList { get; set; } = new();

    public List<BookForListDto> FavouriteBooks { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    public override string ToString()
    {
        return $"User with Id: {Id}" +
               $"\nName: {Name}" +
               $"\nEmail: {Email}" +
               $"\nAddress: {Address}";
    }
}