using LibraryManagement.Application.Contracts.Commons.Primitives.Interfaces;

namespace LibraryManagement.Application.Contracts.Commons.Primitives;

public abstract class EntityDto<TId> : IEntity<TId>
    where TId : struct
{
    private TId _id;

    public TId Id
    {
        get => _id;
        set => _id = value;
    }

    protected EntityDto(TId id)
    {
        _id = id;
    }

    protected EntityDto()
    {
    }
}