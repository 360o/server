using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class CreateOfferRequestValidator : AbstractValidator<CreateOfferRequest>
    {
        public CreateOfferRequestValidator()
        {
            RuleFor(r => new { r.EnglishName, r.FrenchName })
                .Must(r => !string.IsNullOrWhiteSpace(r.EnglishName) || !string.IsNullOrWhiteSpace(r.FrenchName))
                .WithMessage("At least one Name must be defined");

            RuleFor(r => r.OfferItems).NotEmpty();

            RuleForEach(r => r.OfferItems).SetValidator(new CreateOfferItemValidator());

            When(r => r.Discount.HasValue, () =>
            {
                RuleFor(r => r.Discount!.Value).SetValidator(new MoneyValueValidator());
            });
        }
    }

    internal class CreateOfferItemValidator : AbstractValidator<CreateOfferRequestItem>
    {
        public CreateOfferItemValidator()
        {
            RuleFor(i => i.ItemId).NotEmpty();
            RuleFor(i => i.Quantity).GreaterThan(0);
        }
    }
}