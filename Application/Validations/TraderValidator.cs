using Domain.Entities;
using FluentValidation;

namespace Application.Validations
{
    public class TraderValidator : AbstractValidator<Trader>
    {
        public TraderValidator()
        {
            // Validate Name 
            RuleFor(trader => trader.Name)
                .NotEmpty().WithMessage("Trader name is required.")
                .MaximumLength(100).WithMessage("Trader name must not exceed 100 characters.");

            // Validate AccountBalance 
            RuleFor(trader => trader.AccountBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Account balance must be non-negative.");

            // Validate Email
            RuleFor(trader => trader.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }   
}
