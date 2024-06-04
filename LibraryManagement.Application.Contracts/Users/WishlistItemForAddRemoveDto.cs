namespace LibraryManagement.Application.Contracts.Users;

public record WishlistItemForAddRemoveDto(Guid UserId, int BookId);
