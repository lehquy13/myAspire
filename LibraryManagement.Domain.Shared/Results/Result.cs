namespace LibraryManagement.Domain.Shared.Results;

public class Result : IResult
{
    public bool IsSuccess { get; set; }
    
    public string DisplayMessage { get; set; } = string.Empty;
    
    public List<string> ErrorMessages { get; set; } = new();
    
    public bool IsFailure => !IsSuccess;

    private Result(){}

    private Result(bool isSuccess, string message, List<string> errors)
    {
        IsSuccess = isSuccess;
        DisplayMessage = message;
        ErrorMessages = errors;
    }
    
    public static Result Success()
    {
        return new Result()
        {
            IsSuccess = true,
            DisplayMessage = "Success"
        };
    }
    
    public static Result Success(string message)
    {
        return new Result()
        {
            IsSuccess = true,
            DisplayMessage = message
        };
    }

    public static Result Fail(string errorMessage)
    {
        var result = new  Result(
            false,
            errorMessage,
            new List<string> { errorMessage }
        );
        return result;
    }

    public Result WithErrors(List<string> resultErrorMessages)
    {
        ErrorMessages.AddRange(resultErrorMessages);
        return this;
    }
    
    public Result WithError(string resultErrorMessages)
    {
        ErrorMessages.Add(resultErrorMessages);
        return this;
    }
}