using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.Services.Photos
{
    public interface IPhotosService
    {
        Task<IEnumerable<Album>> GetAlbums();
        Task<Album?> GetAlbum(string slug);
        Task<IEnumerable<Photo>> GetPhotosByAlbum(string slug);
        Task<Photo?> GetPhoto(int id);
    }
}
