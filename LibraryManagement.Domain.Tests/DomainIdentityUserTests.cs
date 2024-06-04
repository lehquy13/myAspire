using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Shared.Exceptions.OrderExceptions;
using LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

namespace LibraryManagement.Domain.Tests;

public class DomainIdentityUserTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateIdentityUser_WithValidPhoneNumberAndPasswordHash_ShouldSucceed()
    {
        // Arrange
        string phoneNumber = "1234567890";
        string passwordHash = "hashedPassword";
        
        // Act
        var identityUser = new IdentityUser(phoneNumber, passwordHash);

        // Assert
        Assert.That(identityUser.PhoneNumber, Is.EqualTo(phoneNumber));
        Assert.IsNotNull(identityUser.PasswordHash);
        Assert.IsNotNull(identityUser.PasswordSalt);
    }

    [Test]
    public void CreateIdentityUser_WithEmptyPhoneNumber_ShouldThrowInvalidUserPhoneNumberException()
    {
        // Arrange
        string phoneNumber = string.Empty;
        string passwordHash = "hashedPassword";

        // Act and Assert
        Assert.Throws<InvalidUserPhoneNumberException>(() =>
        {
            var identityUser = new IdentityUser(phoneNumber, passwordHash);
        });
    }

    [Test]
    public void Deposit_WithPositiveAmount_ShouldIncreaseBalanceAmount()
    {
        // Arrange
        var identityUser = new IdentityUser("1234567890", "hashedPassword");
        decimal amount = 50.0m;
        decimal expectedBalance = 50.0m;


        //Act & Assert

        Assert.DoesNotThrow(() => identityUser.Deposit(amount));
        Assert.That(identityUser.BalanceAmount, Is.EqualTo(expectedBalance));
    }

    [Test]
    public void Purchase_WithSufficientBalanceAmount_ShouldDecreaseBalanceAmount()
    {
        // Arrange
        var identityUser = new IdentityUser("1234567890", "hashedPassword");
        decimal initialBalance = 100.0m;
        decimal amountToPurchase = 50.0m;
        identityUser.Deposit(initialBalance);

        // Act & Assert
        Assert.DoesNotThrow(() => identityUser.Purchase(amountToPurchase));
    }

    [Test]
    public void Purchase_WithInsufficientBalanceAmount_ShouldReturnFalse()
    {
        // Arrange
        var identityUser = new IdentityUser("1234567890", "hashedPassword");
        decimal initialBalance = 30.0m;
        decimal amountToPurchase = 50.0m;
        identityUser.Deposit(initialBalance);

        // Act & Assert 
        Assert.Throws<InsufficientBalanceAmountException>(() => identityUser.Purchase(amountToPurchase));
    }

    [Test]
    public void ValidatePassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        string phoneNumber = "1234567890";
        string passwordHash = "hashedPassword";
        var identityUser = new IdentityUser(phoneNumber, passwordHash);
        string passwordToValidate = "hashedPassword";

        // Act
        bool isValid = identityUser.ValidatePassword(passwordToValidate);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void ValidatePassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        string phoneNumber = "1234567890";
        string passwordHash = "hashedPassword";
        var identityUser = new IdentityUser(phoneNumber, passwordHash);
        string passwordToValidate = "incorrectPassword";

        // Act
        bool isValid = identityUser.ValidatePassword(passwordToValidate);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void Deposit_WithNegativeAmount_ShouldThrowInvalidUserBalanceAmountException()
    {
        // Arrange
        var identityUser = new IdentityUser("1234567890", "hashedPassword");
        decimal negativeAmount = -20.0m;

        // Act and Assert
        Assert.Throws<InvalidUserBalanceAmountException>(() => identityUser.Deposit(negativeAmount));
    }

    [Test]
    public void Purchase_WithNegativeAmount_ShouldThrowInvalidPaymentAmountException()
    {
        // Arrange
        var identityUser = new IdentityUser("1234567890", "hashedPassword");
        decimal negativeAmount = -30.0m;

        // Act and Assert
        Assert.Throws<InvalidPaymentAmountException>(() => identityUser.Purchase(negativeAmount));
    }
}