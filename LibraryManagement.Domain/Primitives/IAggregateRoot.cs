namespace LibraryManagement.Domain.Primitives;

public interface IAggregateRoot<TId> : IEntity<TId> where TId : notnull
{
}