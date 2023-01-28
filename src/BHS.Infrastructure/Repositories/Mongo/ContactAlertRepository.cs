using BHS.Contracts;
using BHS.Domain;
using BHS.Domain.ContactUs;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class ContactAlertRepository : IContactAlertRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public ContactAlertRepository(IMongoClient mongoClient, IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _mongoClient = mongoClient;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<ContactAlert> Insert(ContactAlertRequest contactRequest, CancellationToken cancellationToken = default)
    {
        var alert = ContactAlertDto.FromRequest(contactRequest, _dateTimeOffsetProvider.Now());

        await _mongoClient.GetBhsCollection<ContactAlertDto>("contactAlerts")
                .InsertOneAsync(alert, cancellationToken: cancellationToken);

        return alert.ToAlert();
    }

    public async Task Backfill(DateTimeOffset dateCreated, ContactAlertRequest contactRequest, CancellationToken cancellationToken = default)
    {
        var dto = ContactAlertDto.FromRequest(contactRequest, dateCreated);

        await _mongoClient.GetBhsCollection<ContactAlertDto>("contactAlerts")
                .InsertOneAsync(dto, cancellationToken: cancellationToken);
    }
}
