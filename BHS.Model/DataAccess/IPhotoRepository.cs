using BHS.Contracts.Photos;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface IPhotoRepository
    {
        Task<Photo?> GetById(int id);
    }
}
