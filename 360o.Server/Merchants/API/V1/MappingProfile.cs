using System;
using _360o.Server.Merchants.API.V1.Controllers.DTOs;
using _360o.Server.Merchants.API.V1.Model;
using AutoMapper;

namespace _360o.Server.Merchants.API.V1
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<PlaceDTO, Place>();
			CreateMap<Place, PlaceDTO>();
			CreateMap<Merchant, MerchantDTO>();
		}
	}
}

