using _360o.Server.API.V1.Merchants.Controllers.DTOs;
using FluentValidation;

namespace _360o.Server.API.V1.Merchants.Controllers.Validators
{
    public class CreateMerchantRequestValidator : AbstractValidator<CreateMerchantRequest>
    {
        public CreateMerchantRequestValidator()
        {
            RuleForEach(r => r.Places).SetValidator(new CreateMerchantPlaceValidator());
        }

        private class CreateMerchantPlaceValidator : AbstractValidator<CreateMerchantPlace>
        {
            public CreateMerchantPlaceValidator()
            {
                RuleFor(p => p.GooglePlaceId).NotEmpty();
                RuleFor(p => p.FormattedAddress).NotEmpty();
                RuleFor(p => p.Location).NotNull();
            }
        }
    }
}

