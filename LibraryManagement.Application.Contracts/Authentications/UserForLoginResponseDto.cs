namespace LibraryManagement.Application.Contracts.Authentications;

public class UserForLoginResponseDto
{
    public Guid Id { get; set; }

    //User information
    public string Name { get; set; } = string.Empty;

    //Account References
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Learner";
}

