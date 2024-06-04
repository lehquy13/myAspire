namespace LibraryManagement.Domain.Shared.Exceptions.BasketExceptions;

public class BasketNotFoundException : Exception
{
    public override string Message { get; } = "Basket not found";
    
    public BasketNotFoundException(int id)
    {
        Message = $"Basket with id {id} not found";
    }
    
    public BasketNotFoundException()
    {
    }
}