using BHS.Contracts;
using BHS.Domain.ContactUs;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

public class ContactAlertRepository : IContactAlertRepository
{
    private readonly IMongoClient _mongoClient;
    private readonly TimeProvider _timeProvider;

    public ContactAlertRepository(IMongoClient mongoClient, TimeProvider timeProvider)
    {
        _mongoClient = mongoClient;
        _timeProvider = timeProvider;
    }

    public async Task<ContactAlert> Insert(ContactAlertRequest contactRequest, CancellationToken cancellationToken = default)
    {
        var alert = ContactAlertDto.FromRequest(contactRequest, _timeProvider.GetUtcNow());

        await _mongoClient.GetBhsCollection<ContactAlertDto>("contactAlerts")
                .InsertOneAsync(alert, cancellationToken: cancellationToken);

        return alert.ToAlert();
    }
}
