using System;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace _360o.Server.Merchants.API.V1.Model
{
	public class Place : BaseEntity
	{
		public Place(string googlePlaceId, string formattedAddress, Location location)
		{
			GooglePlaceId = googlePlaceId;
			FormattedAddress = formattedAddress;
			Location = location;
		}

		public string GooglePlaceId { get; private set; }

		public string FormattedAddress { get; private set; }

		public Location Location { get; private set; }

		public Guid MerchantId { get; private set; }
		[JsonIgnore]
		public Merchant Merchant { get; private set; }
	}
}

