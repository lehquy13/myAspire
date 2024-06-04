using LibraryManagement.Domain.Library.UserAggregate;

namespace LibraryManagement.Domain.Specifications.Users;

public sealed class UserListQuerySpec : GetListSpecificationBase<User>
{
    public UserListQuerySpec(
        int pageIndex,
        int pageSize)
        : base(pageIndex, pageSize)
    {
    }
}