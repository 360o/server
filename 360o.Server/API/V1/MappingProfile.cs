using _360o.Server.API.V1.Organizations.DTOs;
using _360o.Server.API.V1.Organizations.Model;
using _360o.Server.API.V1.Stores.DTOs;
using _360o.Server.API.V1.Stores.Model;
using AutoMapper;

namespace _360o.Server.API.V1
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Place, PlaceDTO>().ReverseMap();
            CreateMap<Place, CreateStorePlace>().ReverseMap();
            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Organization, OrganizationDTO>();
        }
    }
}

