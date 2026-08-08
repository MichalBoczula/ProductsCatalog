using System.Text.Json.Serialization;

namespace ProductCatalog.Domain.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MobilePhonesBrand
    {
        Apple,
        Motorola,
        Samsung,
        Xiaomi
    }
}