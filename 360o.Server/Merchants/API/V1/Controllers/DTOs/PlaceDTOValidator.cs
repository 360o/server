using System;
using FluentValidation;

namespace _360o.Server.Merchants.API.V1.Controllers.DTOs
{
    public class PlaceDTOValidator : AbstractValidator<PlaceDTO>
    {
        public PlaceDTOValidator()
        {
            RuleFor(p => p.GooglePlaceId).NotEmpty();
            RuleFor(p => p.FormattedAddress).NotEmpty();
            RuleFor(p => p.Location).NotNull();
        }
    }
}

