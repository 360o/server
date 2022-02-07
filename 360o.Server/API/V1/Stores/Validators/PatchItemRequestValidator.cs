using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class PatchItemRequestValidator : AbstractValidator<PatchItemRequest>
    {
        public PatchItemRequestValidator()
        {
            When(r => r.Price.HasValue, () =>
            {
                RuleFor(r => r.Price.Value).SetValidator(new MoneyValueValidator());
            });
        }
    }
}