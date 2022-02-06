using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class ListStoresRequestValidator : AbstractValidator<ListStoresRequest>
    {
        public ListStoresRequestValidator()
        {
            When(r => r.Radius.HasValue || r.Latitude.HasValue || r.Longitude.HasValue, () =>
            {
                RuleFor(r => r.Radius).NotNull().GreaterThan(0);
                RuleFor(r => r.Latitude).NotNull().InclusiveBetween(-90, 90);
                RuleFor(r => r.Longitude).NotNull().InclusiveBetween(-180, 180);
            });
        }
    }
}