namespace LibraryManagement.Domain.Shared.Results;

public interface IResult
{
    public bool IsSuccess { get; set; }
    
    public string DisplayMessage { get; set; }
    
    public List<string> ErrorMessages { get; set; }
    
}