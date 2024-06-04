using LibraryManagement.Domain.Library.UserAggregate;

namespace LibraryManagement.Domain.Specifications.Users;

public class UserFilterByDateSpec : SpecificationBase<User>
{
    public UserFilterByDateSpec(DateTime? fromDate, DateTime? toDate)
    {
        Criteria = order =>
            (fromDate == null || order.CreatedAt >= fromDate) &&
            (toDate == null || order.CreatedAt <= toDate);
        
        //AddInclude();
    }
}