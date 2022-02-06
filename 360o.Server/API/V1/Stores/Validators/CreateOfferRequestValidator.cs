using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequest>
    {
        public CreateOfferRequestValidator()
        {
            RuleFor(r => new { r.EnglishName, r.FrenchName })
                .Must(r => r.EnglishName != null || r.FrenchName != null)
                .WithMessage("At least one Name must be defined");

            RuleFor(r => r.Items).NotEmpty();

            When(r => r.Discount.HasValue, () =>
            {
                RuleFor(r => r.Discount!.Value).SetValidator(new MoneyValueValidator());
            });
        }
    }
}