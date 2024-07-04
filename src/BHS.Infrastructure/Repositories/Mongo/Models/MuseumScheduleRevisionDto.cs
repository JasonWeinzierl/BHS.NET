using BHS.Contracts.Museum;
using MongoDB.Bson;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record MuseumScheduleRevisionDto(
    ObjectId Id,
    DateTimeOffset DateStarted,
    MuseumSchedule Schedule);
