using System.Text.Json.Serialization;

namespace _360o.Server.Api.V1.Stores.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Iso4217CurrencyCode : short
    {
        CAD = 124,
    }
}