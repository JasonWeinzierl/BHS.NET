using BHS.Contracts.Leadership;
using BHS.Domain;
using BHS.Domain.Leadership;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Models;

namespace BHS.Infrastructure.Repositories;

public class LeadershipRepository : ILeadershipRepository
{
    protected IDbExecuter E { get; }

    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public LeadershipRepository(
        IDbExecuter executer,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        E = executer;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<IReadOnlyCollection<Officer>> GetCurrentOfficers(CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<OfficerDto>(DbConstants.BhsConnectionStringName, "leadership.Officer_GetAll", cancellationToken: cancellationToken);
        return results.OrderBy(r => r.SortOrder).Select(r => r.ToDomainModel()).ToList();
    }

    public async Task<IReadOnlyCollection<Director>> GetCurrentDirectors(CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<DirectorDto>(DbConstants.BhsConnectionStringName, "leadership.Director_GetCurrent", new
        {
            startingYear = _dateTimeOffsetProvider.CurrentYear()
        }, cancellationToken);
        return results.Select(r => r.ToDomainModel()).ToList();
    }
}
