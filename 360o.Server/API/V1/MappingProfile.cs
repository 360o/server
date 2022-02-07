using _360o.Server.Api.V1.Organizations.DTOs;
using _360o.Server.Api.V1.Organizations.Model;
using _360o.Server.Api.V1.Organizations.Services.Inputs;
using _360o.Server.Api.V1.Stores.DTOs;
using _360o.Server.Api.V1.Stores.Model;
using _360o.Server.Api.V1.Stores.Services.Inputs;
using AutoMapper;

namespace _360o.Server.Api.V1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AllowNullCollections = true;

            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<OfferItem, OfferItemDTO>().ReverseMap();
            CreateMap<Offer, OfferDTO>().ReverseMap();
            CreateMap<Place, PlaceDTO>().ReverseMap();
            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Organization, OrganizationDTO>().ReverseMap();
            CreateMap<CreateItemRequest, CreateItemInput>();
            CreateMap<PatchItemRequest, PatchItemInput>();
            CreateMap<CreateOfferRequestItem, CreateOrUpdateOfferInputItem>();
            CreateMap<CreateOfferRequest, CreateOrUpdateOfferInput>();
            CreateMap<CreateStoreRequest, CreateStoreInput>();
            CreateMap<PatchStoreRequest, PatchStoreInput>();
            CreateMap<ListStoresRequest, ListStoresInput>();
            CreateMap<PatchOrganizationRequest, PatchOrganizationInput>();
        }
    }
}