using _360o.Server.API.V1.Merchants.Controllers.DTOs;
using _360o.Server.Merchants.API.V1.Model;
using AutoMapper;

namespace _360o.Server.API.V1.Merchants
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Place, PlaceDTO>().ReverseMap();
            CreateMap<Place, CreateMerchantPlace>().ReverseMap();
            CreateMap<Merchant, MerchantDTO>().ReverseMap();
        }
    }
}

