using _360o.Server.Api.V1.Stores.Model;
using FluentValidation;

namespace _360o.Server.Api.V1.Stores.Validators
{
    public class MoneyValueValidator : AbstractValidator<MoneyValue>
    {
        public MoneyValueValidator()
        {
            RuleFor(m => m.CurrencyCode).IsInEnum();
            RuleFor(m => m.Amount).GreaterThanOrEqualTo(0);
        }
    }
}