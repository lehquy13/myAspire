namespace LibraryManagement.Domain.Shared.Results;

public class Result<T> : IResult where T : notnull
{
    public bool IsSuccess { get; set; } = true;
    
    public string DisplayMessage { get; set; } = string.Empty;
    
    public List<string> ErrorMessages { get; set; } = new();
    
    public T Value { get; set; } = default!;

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value
        };
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value
        };
    }
    public static Result<T> Success(T value, string message)
    {
        var result = new Result<T>
        {
            IsSuccess = true,
            Value = value,
            DisplayMessage = message
        };
        return result;
    }

    public static Result<T> Fail(string errorMessage)
    {
        return new Result<T>
        {
            IsSuccess = false,
            DisplayMessage = errorMessage,
            ErrorMessages = new List<string> { errorMessage }
        };
    }

    public static implicit operator Result<T>(Result result)
    {
        return new Result<T>
        {
            IsSuccess = result.IsSuccess,
            DisplayMessage = result.DisplayMessage,
            ErrorMessages = result.ErrorMessages
        };
    }
}