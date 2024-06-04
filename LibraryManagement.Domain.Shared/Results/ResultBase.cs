namespace LibraryManagement.Domain.Shared.Results;

public abstract class ResultBase : IResult
{
    public bool IsSuccess { get; set; } = true;
    
    public string DisplayMessage { get; set; } = string.Empty;
    
    public List<string> ErrorMessages { get; set; } = new();
    
    protected ResultBase()
    {
    }
}