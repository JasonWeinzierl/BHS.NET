using BHS.Contracts;

namespace BHS.Infrastructure.Repositories.Sql.Models;

public record ContactAlertDTO(
    int Id,
    string? Name,
    string EmailAddress,
    string? Message,
    DateTimeOffset? DateRequested,
    DateTimeOffset DateCreated)
{
    public ContactAlert ToDomainModel()
        => new(Id.ToString(), Name, EmailAddress, Message, DateRequested, DateCreated);
}
