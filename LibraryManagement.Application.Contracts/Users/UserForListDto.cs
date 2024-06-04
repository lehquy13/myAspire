using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Users;

public class UserForListDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"User with Id: {Id}, Name: {Name}";
    }
}