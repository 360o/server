using _360o.Server.API.V1.Stores.Controllers.DTOs;
using FluentValidation;

namespace _360o.Server.API.V1.Stores.Controllers.Validators
{
    public class CreateStoreRequestValidator : AbstractValidator<CreateStoreRequest>
    {
        public CreateStoreRequestValidator()
        {
            RuleFor(r => r.DisplayName).NotNull().NotEmpty();
            RuleFor(r => r.Place).SetValidator(new CreateMerchantPlaceValidator());
        }

        private class CreateMerchantPlaceValidator : AbstractValidator<CreateMerchantPlace>
        {
            public CreateMerchantPlaceValidator()
            {
                RuleFor(p => p.GooglePlaceId).NotEmpty();
                RuleFor(p => p.FormattedAddress).NotEmpty();
                RuleFor(p => p.Location).NotNull();
                // See https://docs.mapbox.com/help/glossary/lat-lon/#:~:text=Latitude%20and%20longitude%20are%20a,180%20to%20180%20for%20longitude.
                RuleFor(p => p.Location.Latitude).InclusiveBetween(-90, 90);
                RuleFor(p => p.Location.Longitude).InclusiveBetween(-180, 180);
            }
        }
    }
}

