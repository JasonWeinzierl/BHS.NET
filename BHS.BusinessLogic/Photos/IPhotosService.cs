using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Photos
{
    public interface IPhotosService
    {
        Task<IEnumerable<Album>> GetAlbums(bool doIncludeHidden = false);
        Task<Album> GetAlbum(int id);
        Task<IEnumerable<Photo>> GetPhotosByAlbum(int id);
        Task<Photo> GetPhoto(int id);
    }
}
