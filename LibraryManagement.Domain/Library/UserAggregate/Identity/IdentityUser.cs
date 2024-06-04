using System.Security.Cryptography;
using System.Text;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;
using LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

namespace LibraryManagement.Domain.Library.UserAggregate.Identity;

public sealed class IdentityUser : AggregateRoot<IdentityGuid>
{
    private decimal _balanceAmount = 0;
    private string _phoneNumber = string.Empty;
    private byte[] _passwordHash = null!;
    private byte[] _passwordSalt = null!;

    public decimal BalanceAmount
    {
        get => _balanceAmount;
        private set
        {
            if (value < 0)
            {
                throw new InvalidUserBalanceAmountException();
            }

            _balanceAmount = value;
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new InvalidUserPhoneNumberException();

            _phoneNumber = value;
        }
    }

    public byte[] PasswordHash => _passwordHash;
    
    public byte[] PasswordSalt => _passwordSalt;

    //Navigation

    public User User { get; private set; } = null!;

    public int IdentityRoleId { get; private set; }

    public IdentityRole IdentityRole { get; private set; } = null!;

    public OtpCode OtpCode { get; private set; } = new();

    private IdentityUser()
    {
        ChangePassword("1q2w3E*");
    }

    public IdentityUser(string phoneNumber, string password)
    {
        Id = IdentityGuid.Create();
        PhoneNumber = phoneNumber;
        ChangePassword(password);
    }

    public void Deposit(decimal amount)
    {
        BalanceAmount += amount;
    }

    public void Purchase(decimal amount)
    {
        if (amount > BalanceAmount)
        {
            throw new InsufficientBalanceAmountException();
        }

        if (amount < 0)
        {
            throw new InvalidPaymentAmountException();
        }

        BalanceAmount -= amount;
    }

    /// <summary>
    /// Consider to remove this one
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public IdentityUser WithUser(User user)
    {
        User = user;
        // Id = user.Id;
        return this;
    }

    /// <summary>
    /// Consider to remove this one
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public IdentityUser WithRole(IdentityRole role)
    {
        IdentityRole = role;
        IdentityRoleId = role.Id;
        return this;
    }

    public void GenerateOtpCode()
    {
        OtpCode.Reset();
    }

    public void ChangePassword(string password)
    {
        using var hmac = new HMACSHA512();
        _passwordSalt = hmac.Key;
        _passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool ValidatePassword(string password)
    {
        using var hmac = new HMACSHA512(_passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != _passwordHash[i])
            {
                return false;
            }
        }

        return true;
    }
}