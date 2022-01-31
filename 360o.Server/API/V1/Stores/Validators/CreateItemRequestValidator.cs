using _360o.Server.API.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.API.V1.Stores.Validators
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }
}
