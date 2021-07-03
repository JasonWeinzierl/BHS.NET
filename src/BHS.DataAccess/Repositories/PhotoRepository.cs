using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.Domain.DataAccess;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
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
            return E.QuerySingleOrDefaultAsync<Photo>(Constants.bhsConnectionStringName, "photos.Photo_GetById", new { id });
        }
    }
}
