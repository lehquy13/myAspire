using LibraryManagement.Domain.Shared.Paginations;

namespace LibraryManagement.Domain.Specifications;

public abstract class GetListSpecificationBase<TLEntity> : SpecificationBase<TLEntity>, IPaginated
{
    public int PageIndex { get; private set; }
    
    public int PageSize{ get; private set; }

    public GetListSpecificationBase(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        ApplyPaging((pageIndex - 1) * pageSize, pageSize);
    }
}