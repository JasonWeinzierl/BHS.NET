using System.Text.Json.Serialization;

namespace BHS.Contracts;

/// <summary>
/// The visual style of any notification, banner, or alert.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AlertTheme : byte
{
    None = 0,
    Primary = 1,
    Secondary = 2,
    Success = 3,
    Danger = 4,
    Warning = 5,
    Info = 6,
}
