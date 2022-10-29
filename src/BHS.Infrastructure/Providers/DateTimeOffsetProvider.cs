using BHS.Domain;

namespace BHS.Infrastructure.Providers;

public class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset Now() => DateTimeOffset.Now;
    public int CurrentYear() => Now().Year;
}
