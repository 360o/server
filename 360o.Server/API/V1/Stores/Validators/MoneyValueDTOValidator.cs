using _360o.Server.Api.V1.Stores.DTOs;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class MoneyValueDTOValidator : AbstractValidator<MoneyValueDTO>
    {
        public MoneyValueDTOValidator()
        {
            RuleFor(m => m.CurrencyCode).IsInEnum();
            RuleFor(m => m.Amount).GreaterThanOrEqualTo(0);
        }
    }
}