using System;

namespace _360o.Server.Merchants.API.V1.Model
{
	public class Merchant : BaseEntity
	{
		public Merchant(string userId, string displayName)
        {
			UserId = userId;
			DisplayName = displayName;
        }

		public string UserId { get; private set; }

		public string DisplayName { get; private set; }

		private readonly HashSet<Place> _places = new HashSet<Place>();
		public IReadOnlySet<Place> Places => _places;

		public void AddPlace(Place place)
        {
			_places.Add(place);
        }
	}
}

