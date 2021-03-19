using System;

namespace BHS.Model.Providers
{
    public interface IDateTimeOffsetProvider
    {
        DateTimeOffset Now();
        int CurrentYear();
    }
}
