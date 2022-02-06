using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(r => new { r.EnglishName, r.FrenchName })
                .Must(r => r.EnglishName != null || r.FrenchName != null)
                .WithMessage("At least one of Name must be defined");
        }
    }
}