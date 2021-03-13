using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.Services.Photos
{
    public interface IPhotosService
    {
        Task<IEnumerable<Album>> GetAlbums();
        Task<Album?> GetAlbum(int id);
        Task<IEnumerable<Photo>> GetPhotosByAlbum(int id);
        Task<Photo?> GetPhoto(int id);
    }
}
