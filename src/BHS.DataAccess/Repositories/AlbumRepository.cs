using BHS.Contracts.Photos;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Domain.Repositories;

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
            var (albums, photos) = await E.ExecuteSprocQueryMultiple<AlbumDto, Photo>(DbConstants.BhsConnectionStringName, "photos.AlbumPhotos_GetBySlug", new { slug });
            return albums.SingleOrDefault()?.ToDomainModel(photos);
        }

        public async Task<IReadOnlyCollection<Album>> GetAll()
        {
            var results = await E.ExecuteSprocQuery<AlbumDto>(DbConstants.BhsConnectionStringName, "photos.Album_GetAll");
            return results.Select(r => r.ToDomainModel()).ToList();
        }
    }
}
