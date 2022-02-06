using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(r => new { r.EnglishName, r.FrenchName })
                .Must(r => !string.IsNullOrWhiteSpace(r.EnglishName) || !string.IsNullOrWhiteSpace(r.FrenchName))
                .WithMessage("At least one Name must be defined");
        }
    }
}