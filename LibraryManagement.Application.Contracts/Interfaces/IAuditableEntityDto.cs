namespace LibraryManagement.Application.Contracts.Interfaces;

public interface IAuditableEntityDto
{
    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; }
}