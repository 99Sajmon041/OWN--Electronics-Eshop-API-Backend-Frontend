using System.Text.Json.Serialization;

namespace ElectronicsEshop.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    New,
    Pending,
    Paid,
    Cancelled,
    Shipped,
    Completed
}
