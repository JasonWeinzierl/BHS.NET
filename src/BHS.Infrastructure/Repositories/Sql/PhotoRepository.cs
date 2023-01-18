using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using BHS.Infrastructure.Core;

namespace BHS.Infrastructure.Repositories.Sql;

public class PhotoRepository : IPhotoRepository
{
    protected IDbExecuter E { get; }

    public PhotoRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<Photo?> GetById(int id, CancellationToken cancellationToken = default)
        => await E.ExecuteSprocQuerySingleOrDefault<Photo>(DbConstants.BhsConnectionStringName, "photos.Photo_GetById", new { id }, cancellationToken);
}
