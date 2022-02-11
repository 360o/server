using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class CreateStorePlaceValidator : AbstractValidator<PlaceDTO>
    {
        public CreateStorePlaceValidator()
        {
            RuleFor(p => p.GooglePlaceId).NotEmpty();
            RuleFor(p => p.FormattedAddress).NotEmpty();
            RuleFor(p => p.Location).SetValidator(new LocationDTOValidator());
        }
    }
}