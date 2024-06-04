namespace LibraryManagement.Infrastructure.EmailServices;

public class EmailSettings
{
    public static string SectionName { get; set; } = "EmailSettingNames";
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
    public string? SmtpClient { get; set; }= string.Empty;
    public int Port { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public string ManagerEmail { get; set; } = string.Empty;
}