namespace LibraryManagement.Domain.Primitives;

public interface IEntity<TId> where TId : notnull
{
    TId Id { get; }
}