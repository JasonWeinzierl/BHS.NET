using BHS.Domain.Providers;

namespace BHS.DataAccess.Providers
{
    public class DateTimeOffsetProvider : IDateTimeOffsetProvider
    {
        public DateTimeOffset Now() => DateTimeOffset.Now;
        public int CurrentYear() => Now().Year;
    }
}
