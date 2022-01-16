using System;
using _360o.Server.Merchants.API.V1.Controllers.DTOs;
using _360o.Server.Merchants.API.V1.Model;

namespace _360o.Server.Merchants.API.V1.Controllers
{
	public record struct CreateMerchantRequest
	{
		public string DisplayName { get; set; }

		public ISet<PlaceDTO> Places { get; set; }
	}
}

