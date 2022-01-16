using System;
using _360o.Server.Merchants.API.V1.Controllers.DTOs;
using FluentValidation;

namespace _360o.Server.Merchants.API.V1.Controllers
{
    public class CreateMerchantRequestValidator : AbstractValidator<CreateMerchantRequest>
    {
        public CreateMerchantRequestValidator()
        {
            RuleForEach(r => r.Places).SetValidator(new PlaceDTOValidator());
        }
    }
}

