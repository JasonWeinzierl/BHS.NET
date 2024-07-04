using System.ComponentModel.DataAnnotations;

namespace BHS.Contracts.Museum;

public record MuseumDay(
    [Range(0, 6)] DayOfWeek DayOfWeek,
    string FromTime,
    string ToTime);
