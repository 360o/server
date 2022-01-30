using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Organizations.DTOs;
using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Organizations.Validators;
using _360o.Server.API.V1.Stores.Model;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _360o.Server.API.V1.Organizations.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly ApiContext _apiContext;

        private readonly IMapper _mapper;

        public OrganizationsController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        public async Task<ActionResult<OrganizationDTO>> CreateOrganizationAsync([FromBody] CreateOrganizationRequest request)
        {
            var validator = new CreateOrganizationRequestValidator();

            validator.ValidateAndThrow(request);

            var organization = new Organization(userId: User.Identity.Name, name: request.Name, englishLongDescription: request.EnglishLongDescription, englishShortDescription: request.EnglishShortDescription, englishCategories: request.EnglishCategories, frenchShortDescription: request.FrenchShortDescription, frenchLongDescription: request.FrenchLongDescription, frenchCategories: request.FrenchCategories);

            _apiContext.Add(organization);

            await _apiContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrganizationByIdAsync), new { id = organization.Id }, _mapper.Map<OrganizationDTO>(organization));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Store))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrganizationDTO>> GetOrganizationByIdAsync(Guid id)
        {
            var organization = await _apiContext.Organizations.FindAsync(id);

            if (organization == null)
            {
                return Problem(detail: "Organization not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.ItemNotFound.ToString());
            }

            return _mapper.Map<OrganizationDTO>(organization);
        }
    }
}
