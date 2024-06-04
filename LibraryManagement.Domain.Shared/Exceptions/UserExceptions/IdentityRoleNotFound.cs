namespace LibraryManagement.Domain.Shared.Exceptions.UserExceptions;

public class IdentityRoleNotFoundException : Exception
{
    public override string Message { get; } = "Identity role not found!";
}