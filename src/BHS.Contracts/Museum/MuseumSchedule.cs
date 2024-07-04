namespace BHS.Contracts.Museum;

public record MuseumSchedule(
    IEnumerable<MuseumDay> Days,
    MuseumMonthRange Months);
