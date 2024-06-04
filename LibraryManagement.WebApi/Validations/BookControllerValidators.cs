using FluentValidation;
using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.WebApi.Models;

namespace LibraryManagement.WebApi.Validations;

public class BookForUpsertDtoValidator : AbstractValidator<BookForUpsertDto>
{
    public BookForUpsertDtoValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.CurrentPrice)
            .GreaterThan(0).WithMessage("CurrentPrice must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.AuthorIds)
            .NotEmpty().WithMessage("AuthorIds are required.")
            .Must(ids => ids.Count > 0).WithMessage("At least one AuthorId is required.");

        RuleFor(x => x.Genre)
            .NotEmpty().WithMessage("Genre is required.")
            .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.PublicationDate)
            .NotEmpty().WithMessage("PublicationDate is required.")
            .Must(BeAValidDate).WithMessage("Invalid PublicationDate.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .Matches(@"\.(jpeg|jpg|gif|png)$").WithMessage("Invalid image URL format.");
    }

    private bool BeAValidDate(DateTime date)
    {
        return date < DateTime.Now;
    }
}

public class AddReviewRequestValidator : AbstractValidator<AddReviewRequest>
{
    public AddReviewRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MinimumLength(10).WithMessage("Content must be at least 10 characters.")
            .MaximumLength(500).WithMessage("Content cannot exceed 500 characters.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .Matches(@"\.(jpeg|jpg|gif|png)$").WithMessage("Invalid image URL format.");
    }
}