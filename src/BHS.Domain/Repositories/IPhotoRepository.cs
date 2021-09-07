using BHS.Contracts.Photos;

namespace BHS.Domain.Repositories
{
    public interface IPhotoRepository
    {
        Task<Photo?> GetById(int id);
    }
}
