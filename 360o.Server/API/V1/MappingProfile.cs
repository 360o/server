using _360o.Server.API.V1.Organizations.DTOs;
using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Organizations.Services;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using _360o.Server.API.V1.Stores.Services;
using AutoMapper;
using static _360o.Server.API.V1.Stores.Services.CreateStoreInput;

namespace _360o.Server.API.V1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateStoreRequestPlace, CreateStoreInputPlace>();
            CreateMap<CreateStoreRequest, CreateStoreInput>();
            CreateMap<Place, PlaceDTO>();
            CreateMap<Store, StoreDTO>();
            CreateMap<ListStoresRequest, ListStoresInput>();

            CreateMap<CreateOrganizationInput, Organization>();
            CreateMap<Organization, OrganizationDTO>();
        }
    }
}

