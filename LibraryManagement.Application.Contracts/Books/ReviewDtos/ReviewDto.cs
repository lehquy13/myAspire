using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Books.ReviewDtos;

public class ReviewForListDto : EntityDto<int>
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Username { get; set; } = null!;

    public DateTime ReviewDate { get; set; } = DateTime.Now;

    public bool IsLike { get; set; } = false;

    public int Rating { get; set; } = 5;

    public string ImageUrl { get; set; } = string.Empty;

    public string BookTitle { get; set; } = null!;

    public override string ToString()
    {
        return $"Review with Id: {Id}" +
               $"\nTitle: {Title}" +
               $"\nUsername: {Username}" +
               $"\nContent: {Content}" +
               $"\nReviewDate: {ReviewDate.ToShortTimeString()}" +
               $"\nIsLike: {IsLike}" +
               $"\nRating: {Rating}";
    }
}