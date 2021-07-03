using System;

namespace BHS.Domain.Providers
{
    public interface IDateTimeOffsetProvider
    {
        DateTimeOffset Now();
        int CurrentYear();
    }
}
