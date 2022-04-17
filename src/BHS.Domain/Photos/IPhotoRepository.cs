using BHS.Contracts.Photos;

namespace BHS.Domain.Photos
{
    public interface IPhotoRepository
    {
        Task<Photo?> GetById(int id);
    }
}
