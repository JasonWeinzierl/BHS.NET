using BHS.Contracts.Photos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Photos
{
    public interface IPhotosService
    {
        IAsyncEnumerable<Album> GetAlbums(bool doIncludeHidden = false);
        Task<Album> GetAlbum(int id);
        IAsyncEnumerable<Photo> GetPhotosByAlbum(int id);
        Task<Photo> GetPhoto(int id);
    }
}
