using System;
using Microsoft.AspNetCore.Mvc;

namespace _360o.Server.Merchants.API.V1.Controllers
{
	public class ListMerchantsRequest
	{
		public double? Latitude { get; set; }

		public double? Longitude { get; set; }

		public double? Radius { get; set; }
	}
}

