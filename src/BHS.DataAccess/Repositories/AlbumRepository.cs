using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Model.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        protected IDbExecuter E { get; }

        public AlbumRepository(IDbExecuter executer)
        {
            E = executer;
        }

        public async Task<AlbumPhotos?> GetBySlug(string slug)
        {
            var (albums, photos) = await E.QueryMultipleAsync<AlbumDto, Photo>(Constants.bhsConnectionStringName, "photos.AlbumPhotos_GetBySlug", new { slug });
            return albums.SingleOrDefault()?.ToDomainModel(photos);
        }

        public async Task<IEnumerable<Album>> GetAll()
        {
            var results = await E.QueryAsync<AlbumDto>(Constants.bhsConnectionStringName, "photos.Album_GetAll");
            return results.Select(r => r.ToDomainModel());
        }
    }
}
