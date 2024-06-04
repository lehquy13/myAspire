using LibraryManagement.Domain.Primitives;

namespace LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;

public class OrderDetailGuid : ValueObject
{
    public Guid Value { get; } 

    private OrderDetailGuid(Guid value)
    {
        Value = value;
    }

    private OrderDetailGuid()
    {
        
    }
    
    public static OrderDetailGuid Create()
    {
        return new OrderDetailGuid(Guid.NewGuid());
    }
    
    public static OrderDetailGuid Create(Guid value)
    {
        return new OrderDetailGuid(value);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}