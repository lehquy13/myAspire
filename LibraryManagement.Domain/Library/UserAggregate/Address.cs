using LibraryManagement.Domain.Primitives;

namespace LibraryManagement.Domain.Library.UserAggregate;

public class Address : ValueObject
{
    public string City { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public Address()
    {
        
    }
    
    public Address(string city, string country)
    {
        City = city;
        Country = country;
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Country;
    }
}