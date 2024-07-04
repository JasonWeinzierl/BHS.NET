using System.ComponentModel.DataAnnotations;

namespace BHS.Contracts.Museum;

public record MuseumMonthRange(
    [Range(1, 12)] int StartMonth,
    [Range(1, 12)] int EndMonth);
