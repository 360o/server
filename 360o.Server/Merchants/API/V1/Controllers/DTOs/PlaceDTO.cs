using System;
using _360o.Server.Merchants.API.V1.Model;

namespace _360o.Server.Merchants.API.V1.Controllers.DTOs
{
	public record struct PlaceDTO
	{
		public Guid Id { get; set; }

		public string GooglePlaceId { get; set; }

		public string FormattedAddress { get; set; }

		public Location Location { get; set; }
	}
}

