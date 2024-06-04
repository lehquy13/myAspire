using System.Linq.Expressions;

namespace LibraryManagement.Domain.Specifications;

public abstract class SpecificationBase<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; init; } = null;
    public List<Expression<Func<T, object>>> IncludeExpressions { get; } = new();
    public List<string> IncludeStrings { get; } = new();

    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }
    
    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}