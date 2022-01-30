using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Stores.Model;
using AutoMapper;

namespace _360o.Server.API.V1.Organizations.Services
{
    public class OrganizationsService : IOrganizationsService
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;

        public OrganizationsService(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        public async Task<Organization> CreateOrganizationAsync(CreateOrganizationInput input)
        {
            var organization = _mapper.Map<Organization>(input);

            _apiContext.Add(organization);

            await _apiContext.SaveChangesAsync();

            return organization;
        }

        public async Task<Organization?> GetOrganizationByIdAsync(Guid id)
        {
            return await _apiContext.Organizations.FindAsync(id);
        }
    }
}
