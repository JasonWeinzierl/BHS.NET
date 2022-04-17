using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using BHS.Infrastructure.Core;

namespace BHS.Infrastructure.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        protected IDbExecuter E { get; }

        public PhotoRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public Task<Photo?> GetById(int id)
        {
            return E.ExecuteSprocQuerySingleOrDefault<Photo>(DbConstants.BhsConnectionStringName, "photos.Photo_GetById", new { id });
        }
    }
}
