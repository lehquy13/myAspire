using LibraryManagement.Domain.Primitives;

namespace LibraryManagement.Domain.Library.OrderAggregate.ValueObjects;

public class OrderGuid : ValueObject
{
    public Guid Value { get; } 

    private OrderGuid()
    {
    }
    
    private OrderGuid(Guid value)
    {
        Value = value;
    }
    
    public static OrderGuid Create()
    {
        return new OrderGuid(Guid.NewGuid());
    }
    
    public static OrderGuid Create(Guid guid)
    {
        return new OrderGuid(guid);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}