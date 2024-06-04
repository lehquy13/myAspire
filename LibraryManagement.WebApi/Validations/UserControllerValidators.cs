using FluentValidation;
using LibraryManagement.WebApi.Models;

namespace LibraryManagement.WebApi.Validations;

public class DepositRequestValidator : AbstractValidator<DepositRequest>
{
    public DepositRequestValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
