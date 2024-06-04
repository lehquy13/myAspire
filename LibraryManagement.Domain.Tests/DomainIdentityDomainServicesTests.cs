using LibraryManagement.Domain.DomainServices;
using LibraryManagement.Domain.DomainServices.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BasketAggregate;
using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using LibraryManagement.Test.Shared;
using Moq;

namespace LibraryManagement.Domain.Tests;

public class DomainIdentityDomainServicesTests
{
    private IdentityDomainServices _identityDomainServices;
    private readonly Mock<IIdentityRepository> _identityUserRepositoryMock = new();
    private readonly Mock<IRepository<IdentityRole, int>> _identityRoleRepositoryMock = new();
    private readonly Mock<IBasketRepository> _basketRepositoryMock = new();
    private readonly Mock<IAppLogger<IdentityDomainServices>> _loggerMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    public DomainIdentityDomainServicesTests()
    {
        _identityDomainServices = new IdentityDomainServices(
            _identityUserRepositoryMock.Object,
            _identityRoleRepositoryMock.Object,
            _basketRepositoryMock.Object,
            _loggerMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Test]
    public async Task SignInAsync_ReturnsNull_WhenUserNotFound()
    {
        // Arrange
        var email = "user@example.com";
        var password = "password";

        _identityUserRepositoryMock.Setup(repo => repo.FindByEmailAsync(email))
            .ReturnsAsync((IdentityUser)null); // Simulate user not found

        // Act
        var result = await _identityDomainServices.SignInAsync(email, password);

        // Assert
        Assert.Null(result);
    }

    [Test]
    public async Task SignInAsync_ReturnsNull_WhenPasswordInvalid()
    {
        // Arrange
        var email = "user@example.com";
        var password = "password";
        var identityUser = new IdentityUser(email, "hashedPassword");

        _identityUserRepositoryMock.Setup(repo => repo.FindByEmailAsync(email))
            .ReturnsAsync(identityUser);

        // Act
        var result = await _identityDomainServices.SignInAsync(email, "wrongPassword"); // Incorrect password

        // Assert
        Assert.Null(result);
    }

    [Test]
    public async Task SignInAsync_ReturnsIdentityUser_WhenCredentialsValid()
    {
        // Arrange
        var email = "user@example.com";
        var password = "password";
        var identityUser = new IdentityUser(email, "password");

        _identityUserRepositoryMock.Setup(repo => repo.FindByEmailAsync(email))
            .ReturnsAsync(identityUser);

        // Act
        var result = await _identityDomainServices.SignInAsync(email, password);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public async Task CreateAsync_SuccessfulUserCreation_ReturnsValidIdentityUser()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var email = "email@emai.com";
        var password = "password";
        var phoneNumber = "phoneNumber";
        var name = "name";
        var city = "city";
        var country = "country";
        var identityRole = TestShared.IdentityRoles[0];

        _identityRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(identityRole);

        _identityUserRepositoryMock.Setup(repo => repo.FindByEmailAsync(email))
            .ReturnsAsync(new IdentityUser(email, password));

        _identityUserRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(true);

        _basketRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Basket>()))
            .ReturnsAsync(true);

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.CreateAsync(email, password, phoneNumber, name, city, country);

        // Assert
        Assert.IsTrue(result.IsSuccess); // Check if the result is a success
        Assert.IsNotNull(result.Value); // Ensure the created IdentityUser is not null
        Assert.That(phoneNumber, Is.EqualTo(result.Value.PhoneNumber));
        Assert.That(name, Is.EqualTo(result.Value.User.Name));
        Assert.That(city, Is.EqualTo(result.Value.User.Address.City));
        Assert.That(country, Is.EqualTo(result.Value.User.Address.Country));
        Assert.That(email, Is.EqualTo(result.Value.User.Email));
    }

    [Test]
    public async Task CreateAsync_ReturnsFail_WhenInsertUserFail()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var email = "email@emai.com";
        var password = "password";
        var phoneNumber = "phoneNumber";
        var name = "name";
        var city = "city";
        var country = "country";
        var identityRole = TestShared.IdentityRoles[0];

        _identityRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(identityRole);

        _identityUserRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(false);

        _basketRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Basket>()));

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.CreateAsync(email, password, phoneNumber, name, city, country);

        // Assert
        Assert.IsFalse(result.IsSuccess); // Check if the result is a success
        Assert.IsNull(result.Value); // Ensure the created IdentityUser is not null
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.InsertUserFail));
    }

    [Test]
    public async Task CreateAsync_ReturnsFail_WhenInsertUserBasketFail()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var email = "email@emai.com";
        var password = "password";
        var phoneNumber = "phoneNumber";
        var name = "name";
        var city = "city";
        var country = "country";
        var identityRole = TestShared.IdentityRoles[0];

        _identityRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(identityRole);

        _identityUserRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(true);

        _basketRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Basket>())).ReturnsAsync(false);

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.CreateAsync(email, password, phoneNumber, name, city, country);

        // Assert
        Assert.IsFalse(result.IsSuccess); // Check if the result is a success
        Assert.IsNull(result.Value); // Ensure the created IdentityUser is not null
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.InsertUserBasketFail));
    }

    [Test]
    public async Task CreateAsync_ReturnsFail_WhenRoleNotFound()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var email = "email@emai.com";
        var password = "password";
        var phoneNumber = "phoneNumber";
        var name = "name";
        var city = "city";
        var country = "country";
        IdentityRole? identityRole = null;

        _identityRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(identityRole);

        _identityUserRepositoryMock.Setup(repo => repo.FindByEmailAsync(email))
            .ReturnsAsync(new IdentityUser(email, password));

        _basketRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Basket>()));

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.CreateAsync(email, password, phoneNumber, name, city, country);

        // Assert
        Assert.IsFalse(result.IsSuccess); // Check if the result is a success
        Assert.IsNotNull(result); // Ensure the created IdentityUser is not null
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.RoleNotFound));
    }

    [Test]
    public async Task ChangePassword_ReturnSuccess_WhenValid()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var identityUser = TestShared.IdentityUsers[0];
        var password = "1231231";
        var newPassword = "phoneNumber";
        var identityRole = TestShared.IdentityRoles[0];

        _identityRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(identityRole);

        _identityUserRepositoryMock.Setup(repo => repo.GetByIdAsync(identityUser.Id))
            .ReturnsAsync(identityUser);

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.ChangePassword(identityUser.Id, password, newPassword);

        // Assert
        Assert.IsTrue(result.IsSuccess); // Check if the result is a success
        Assert.IsNotNull(result);
    }

    [Test]
    public async Task ChangePassword_ReturnFail_WhenUserNotFound()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        IdentityUser? identityUser = null;
        var password = "1231231";
        var newPassword = "phoneNumber";
        var identityRole = TestShared.IdentityRoles[0];

        _identityRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(identityRole);

        _identityUserRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<IdentityGuid>()))
            .ReturnsAsync(identityUser);

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.ChangePassword(IdentityGuid.Create(), password, newPassword);

        // Assert
        Assert.IsFalse(result.IsSuccess); // Check if the result is a success
        Assert.IsNotNull(result);
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.UserNotFound));
    }

    [Test]
    public async Task ChangePassword_ReturnFail_WhenPasswordWrong()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var identityUser = TestShared.IdentityUsers[0];
        var password = "falsePassword";
        var newPassword = "phoneNumber";
        IdentityRole? identityRole = null;

        _identityRoleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(identityRole);

        _identityUserRepositoryMock.Setup(repo => repo.GetByIdAsync(identityUser.Id))
            .ReturnsAsync(identityUser);

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.ChangePassword(identityUser.Id, password, newPassword);

        // Assert
        Assert.IsFalse(result.IsSuccess); // Check if the result is a success
        Assert.IsNotNull(result);
        Assert.That(result.DisplayMessage, Is.EqualTo(DomainServiceErrors.InvalidPassword));
    }
    
    [Test]
    public async Task FindByEmailAsync_ReturnSuccess_WhenValid()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var identityUser = TestShared.IdentityUsers[0];
        var email = "email@gmail.com";

        _identityUserRepositoryMock.Setup(repo => repo.FindByEmailAsync(email))
            .ReturnsAsync(identityUser);

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.FindByEmailAsync(email);

        // Assert
        Assert.IsNotNull(result);
    }
    
    [Test]
    public async Task GetUserIdAsync_ReturnSuccess_WhenValid()
    {
        // Arrange - Mock the necessary repositories and setup for successful user creation
        var identityUser = TestShared.IdentityUsers[0];
        var email = "email@gmail.com";

        _identityUserRepositoryMock.Setup(repo => repo.GetByIdAsync(identityUser.Id))
            .ReturnsAsync(identityUser);

        // Act - Call the CreateAsync method with valid parameters
        var result =
            await _identityDomainServices.FindByEmailAsync(email);

        // Assert
        Assert.IsNotNull(result);
    }
}