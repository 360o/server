using _360o.Server.API.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.API.V1.Stores.Validators
{
    public class CreateStoreRequestValidator : AbstractValidator<CreateStoreRequest>
    {
        public CreateStoreRequestValidator()
        {
            RuleFor(r => r.OrganizationId).NotEmpty();
            RuleFor(r => r.Place).SetValidator(new CreateStorePlaceValidator());
        }


    }
}

