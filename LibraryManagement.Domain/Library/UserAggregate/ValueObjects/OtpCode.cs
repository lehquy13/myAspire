using LibraryManagement.Domain.Primitives;

namespace LibraryManagement.Domain.Library.UserAggregate.ValueObjects;

public class OtpCode : ValueObject
{
    public string Value { get; private set; } = string.Empty;
    public DateTime ExpiredTime { get; private set; } = DateTime.MinValue;
    private const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";

    public void Reset()
    {
        Random rd = new Random();
        char[] chars = new char[6];

        for (int i = 0; i < 6; i++)
        {
            chars[i] = AllowedChars[rd.Next(0, AllowedChars.Length)];
        }

        Value = new string(chars);
        ExpiredTime = DateTime.Now.AddMinutes(15);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public bool IsExpired() => ExpiredTime < DateTime.Now;
}