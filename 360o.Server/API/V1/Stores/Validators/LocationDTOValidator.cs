using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class LocationDTOValidator : AbstractValidator<LocationDTO>
    {
        public LocationDTOValidator()
        {
            // See https://docs.mapbox.com/help/glossary/lat-lon/#:~:text=Latitude%20and%20longitude%20are%20a,180%20to%20180%20for%20longitude
            RuleFor(l => l.Latitude).InclusiveBetween(-90, 90);
            RuleFor(l => l.Longitude).InclusiveBetween(-180, 180);
        }
    }
}