using LibraryManagement.Application.Contracts.Users;
using LibraryManagement.Domain.Shared.Paginations;
using LibraryManagement.Domain.Shared.Results;

namespace LibraryManagement.Application.Contracts.Interfaces;

public interface IUserServices
{
    Task<PaginationResult<UserForListDto>> GetUsers(PaginatedParams userFilterParams);
    
    Task<Result<UserForDetailDto>> GetUserDetailByIdAsync(Guid id);
    
    Task<Result<UserForBasicDto>> GetUserBasicByIdAsync(Guid id);
    
    Task<Result> UpsertUserAsync(UserForUpsertDto userForUpsertDto);
    
    Task<Result> DeleteUserAsync(Guid id);
    
    Task<Result<decimal>> DepositAsync(Guid id, decimal amount);
}