namespace LibraryManagement.Domain.Primitives;

public abstract class Entity<TId> : IEntity<TId> where TId : notnull
{
    private TId _id;

    public virtual TId Id
    {
        get => _id;
        protected init => _id = value;
    }

    protected Entity(TId id)
    {
        _id = id;
    }

#pragma warning disable CS8618

    protected Entity()
    {
    }

#pragma warning restore CS8618
}