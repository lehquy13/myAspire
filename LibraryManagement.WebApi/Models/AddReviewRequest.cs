using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.WebApi.Models;

public class AddReviewRequest
{
    public string Title { get; set; } = null!;
    
    public string Content { get; set; } = null!;

    [Required] 
    public bool IsLike { get; set; } = false;
    
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; } 
    
    public string ImageUrl { get; set; } = null!;
}