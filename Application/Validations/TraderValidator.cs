using Domain.Entities;
using FluentValidation;

namespace Application.Validations
{
    public class TraderValidator : AbstractValidator<Trader>
    {
        public TraderValidator()
        {
            // Validate Name (must not be null or empty)
            RuleFor(trader => trader.Name)
                .NotEmpty().WithMessage("Trader name is required.")
                .MaximumLength(100).WithMessage("Trader name must not exceed 100 characters.");

            // Validate AccountBalance (must be non-negative)
            RuleFor(trader => trader.AccountBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Account balance must be non-negative.");

        
        }
    }   
}
