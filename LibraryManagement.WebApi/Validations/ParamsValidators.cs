using FluentValidation;
using LibraryManagement.Application.Contracts.Order;
using LibraryManagement.Domain.Shared.Paginations;
using LibraryManagement.Domain.Shared.Params;

namespace LibraryManagement.WebApi.Validations;

public class PaginatedParamsValidator : AbstractValidator<PaginatedParams>
{
    public PaginatedParamsValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
    }
}

public class OrderPaginatedParamsValidator : AbstractValidator<OrderPaginatedParams>
{
    public OrderPaginatedParamsValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
        
        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId cannot be empty.");
    }
}

public class BookFilterParamsValidator : AbstractValidator<BookFilterParams>
{
    public BookFilterParamsValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
        
        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.AuthorName)
            .MaximumLength(100).WithMessage("AuthorName cannot exceed 100 characters.");

        RuleFor(x => x.Genre)
            .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters.");
    }
}
