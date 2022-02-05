using _360o.Server.API.V1.Organizations.DTOs;
using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Organizations.Services.Inputs;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using _360o.Server.API.V1.Stores.Services.Inputs;
using AutoMapper;

namespace _360o.Server.API.V1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AllowNullCollections = true;

            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Place, PlaceDTO>().ReverseMap();
            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
            CreateMap<CreateItemRequest, CreateItemInput>();
            CreateMap<UpdateItemRequest, UpdateItemInput>();
            CreateMap<CreateStoreRequest, CreateStoreInput>();
            CreateMap<UpdateStoreRequest, UpdateStoreInput>();
            CreateMap<ListStoresRequest, ListStoresInput>();
            CreateMap<UpdateOrganizationRequest, UpdateOrganizationInput>();
        }
    }
}