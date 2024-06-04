namespace LibraryManagement.Domain.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId> where TId : notnull
{
    protected AggregateRoot(TId id) : base(id)
    {
    }

    protected AggregateRoot()
    {
    }
}