using _360o.Server.Api.V1.Organizations.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Organizations.Validators
{
    public class CreateOrganizationRequestValidator : AbstractValidator<CreateOrganizationRequest>
    {
        public CreateOrganizationRequestValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }
}