using System.Text.Json.Serialization;

namespace _360o.Server.API.V1.Stores.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Iso4217CurrencyCode : short
    {
        CAD = 124,
    }
}
