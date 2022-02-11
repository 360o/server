using _360o.Server.Api.V1.Errors.Enums;
using _360o.Server.Api.V1.Organizations.DTOs;
using _360o.Server.Api.V1.Organizations.Model;
using _360o.Server.Api.V1.Organizations.Services;
using _360o.Server.Api.V1.Organizations.Services.Inputs;
using _360o.Server.Api.V1.Organizations.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace _360o.Server.Api.V1.Organizations.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationsService _organizationsService;

        public OrganizationsController(IOrganizationsService organizationsService)
        {
            _organizationsService = organizationsService;
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

            return CreatedAtAction(nameof(GetOrganizationByIdAsync), new { id = organization.Id }, ToOrganizationDTO(organization));
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

            return ToOrganizationDTO(organization);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrganizationDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<OrganizationDTO>> PatchOrganizationAsync(Guid id, [FromBody] JsonPatchDocument<Organization> patchDoc)
        {
            if (patchDoc != null)
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

                try
                {
                    patchDoc.ApplyTo(organization, ModelState);
                }
                catch (JsonSerializationException ex)
                {
                    if (ex.InnerException is ArgumentException)
                    {
                        return Problem(detail: ex.InnerException.Message, statusCode: (int)HttpStatusCode.BadRequest, title: ErrorCode.InvalidRequest.ToString());
                    }

                    throw;
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                organization = await _organizationsService.UpdateOrganizationAsync(organization);

                return ToOrganizationDTO(organization);
            }
            else
            {
                return BadRequest(ModelState);
            }
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

        private OrganizationDTO ToOrganizationDTO(Organization organization)
        {
            return new OrganizationDTO
            {
                Id = organization.Id,
                Name = organization.Name,
                EnglishShortDescription = organization.EnglishShortDescription,
                EnglishLongDescription = organization.EnglishLongDescription,
                EnglishCategories = organization.EnglishCategories,
                FrenchShortDescription = organization.FrenchShortDescription,
                FrenchLongDescription = organization.FrenchLongDescription,
                FrenchCategories = organization.FrenchCategories,
            };
        }
    }
}