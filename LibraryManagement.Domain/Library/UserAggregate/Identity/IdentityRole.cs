using LibraryManagement.Domain.Primitives;

namespace LibraryManagement.Domain.Library.UserAggregate.Identity;

public class IdentityRole : AggregateRoot<int>
{
    string _name = string.Empty;
    public IdentityRole()
    {
        
    }
    public string Name
    {
        get => _name;
        set => _name = value;
    }
}