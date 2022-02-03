using _360o.Server.API.V1.Errors.Enums;
using _360o.Server.API.V1.Organizations.DTOs;
using _360o.Server.API.V1.Organizations.Services;
using _360o.Server.API.V1.Organizations.Services.Inputs;
using _360o.Server.API.V1.Organizations.Validators;
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
        private readonly IOrganizationsService _organizationsService;
        private readonly IMapper _mapper;

        public OrganizationsController(IOrganizationsService organizationsService, IMapper mapper)
        {
            _organizationsService = organizationsService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrganizationDTO))]
        [Authorize]
        public async Task<ActionResult<OrganizationDTO>> CreateOrganizationAsync([FromBody] CreateOrganizationRequest request)
        {
            var validator = new CreateOrganizationRequestValidator();

            validator.ValidateAndThrow(request);

            var organization = await _organizationsService.CreateOrganizationAsync(
                new CreateOrganizationInput(
                    User.Identity.Name,
                    request.Name,
                    request.EnglishShortDescription,
                    request.EnglishLongDescription,
                    request.EnglishCategories,
                    request.FrenchShortDescription,
                    request.FrenchLongDescription,
                    request.FrenchCategories
                    )
                );

            return CreatedAtAction(nameof(GetOrganizationByIdAsync), new { id = organization.Id }, _mapper.Map<OrganizationDTO>(organization));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganizationDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrganizationDTO>> GetOrganizationByIdAsync(Guid id)
        {
            var organization = await _organizationsService.GetOrganizationByIdAsync(id);

            if (organization == null)
            {
                return Problem(detail: "Organization not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            return _mapper.Map<OrganizationDTO>(organization);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganizationDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrganizationDTO>> UpdateOrganizationAsync(Guid id, [FromBody] UpdateOrganizationRequest request)
        {
            var organization = await _organizationsService.GetOrganizationByIdAsync(id);

            if (organization == null)
            {
                return Problem(detail: "Organization not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            organization = await _organizationsService.UpdateOrganizationAsync(
                _mapper.Map<UpdateOrganizationInput>(request) with { 
                    OrganizationId = organization.Id 
                });

            return _mapper.Map<OrganizationDTO>(organization);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<OrganizationDTO>> DeleteOrganizationByIdAsync(Guid id)
        {
            var organization = await _organizationsService.GetOrganizationByIdAsync(id);

            if (organization == null)
            {
                return Problem(detail: "Organization not found", statusCode: (int)HttpStatusCode.NotFound, title: ErrorCode.NotFound.ToString());
            }

            if (User.Identity.Name != organization.UserId)
            {
                return Forbid();
            }

            await _organizationsService.DeleteOrganizationByIdAsync(id);

            return NoContent();
        }
    }
}
