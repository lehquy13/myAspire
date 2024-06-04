namespace LibraryManagement.Application.Contracts.Books.ReviewDtos;

public record ReviewForCreateDto(
    string Title,
    string Content,
    string ImageUrl,
    int BookId,
    Guid CustomerId,
    bool IsLike,
    int Rating);