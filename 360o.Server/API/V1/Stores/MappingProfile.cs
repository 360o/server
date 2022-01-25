using _360o.Server.API.V1.Stores.Controllers.DTOs;
using _360o.Server.API.V1.Stores.Model;
using AutoMapper;

namespace _360o.Server.API.V1.Stores
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Place, PlaceDTO>().ReverseMap();
            CreateMap<Place, CreateMerchantPlace>().ReverseMap();
            CreateMap<Store, StoreDTO>().ReverseMap();
        }
    }
}

