using _360o.Server.API.V1.Stores.Controllers.DTOs;
using FluentValidation;

namespace _360o.Server.API.V1.Stores.Controllers.Validators
{
    public class CreateStoreRequestValidator : AbstractValidator<CreateStoreRequest>
    {
        public CreateStoreRequestValidator()
        {
            RuleFor(r => r.Place).SetValidator(new CreateMerchantPlaceValidator());
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

