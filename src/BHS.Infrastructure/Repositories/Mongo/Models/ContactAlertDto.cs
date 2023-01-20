using BHS.Contracts;
using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record ContactAlertDto(
    ObjectId Id,
    string? Name,
    string EmailAddress,
    string? Message,
    DateTimeOffset? DateRequested,
    DateTimeOffset DateCreated)
{
    public static ContactAlertDto FromRequest(ContactAlertRequest request, DateTimeOffset dateCreated)
        => new(ObjectId.GenerateNewId(dateCreated.UtcDateTime), request.Name, request.EmailAddress, request.Message, request.DateRequested, dateCreated);

    public ContactAlert ToAlert()
        => new(Id.ToString(), Name, EmailAddress, Message, DateRequested, DateCreated);
}
