namespace LibraryManagement.Domain.Interfaces;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; }
}