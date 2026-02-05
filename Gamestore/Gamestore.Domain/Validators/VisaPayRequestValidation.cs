using FluentValidation;
using Gamestore.Domain.Models.DTO.Payment.Strategy;

namespace Gamestore.Domain.Validators;

public class VisaPayRequestValidation : AbstractValidator<VisaPayDto>
{
    public VisaPayRequestValidation()
    {
        RuleFor(x => x.Holder)
            .NotEmpty().WithMessage("Card holder name is required.")
            .MinimumLength(2)
            .MaximumLength(100).WithMessage("Card holder name cannot exceed 100 characters.");
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required.")
            .CreditCard().WithMessage("Invalid card number format.");
        RuleFor(x => x.MonthExpire)
            .InclusiveBetween(1, 12).WithMessage("Expiration month must be between 1 and 12.");
        RuleFor(x => x.YearExpire)
            .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Expiration year cannot be in the past.");
        RuleFor(x => x.Cvv2)
            .NotEmpty().WithMessage("CVV2 code is required.")
            .ExclusiveBetween(100, 9999).WithMessage("CVV2 code must be 3 or 4 digits.");
    }
}
