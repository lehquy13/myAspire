using FluentValidation;
using LibraryManagement.WebApi.Models;

namespace LibraryManagement.WebApi.Validations;

public class AddItemToBasketRequestValidator : AbstractValidator<AddItemToBasketRequest>
{
    public AddItemToBasketRequestValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}

public class UpdateItemQuantitiesRequestValidator : AbstractValidator<UpdateItemQuantitiesRequest>
{
    public UpdateItemQuantitiesRequestValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}