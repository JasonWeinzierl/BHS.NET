namespace BHS.Domain;

public interface IDateTimeOffsetProvider
{
    DateTimeOffset Now();
    int CurrentYear();
}
