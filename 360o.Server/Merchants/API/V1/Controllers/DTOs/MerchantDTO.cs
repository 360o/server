using System;
namespace _360o.Server.Merchants.API.V1.Controllers.DTOs
{
	public record struct MerchantDTO
	{
		public Guid Id { get; set; }

		public string UserId { get; set; }

		public ISet<PlaceDTO> Places { get; set; }
	}
}

