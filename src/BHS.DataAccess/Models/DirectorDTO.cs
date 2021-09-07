using BHS.Contracts.Leadership;

namespace BHS.DataAccess.Models
{
    public record DirectorDto(string Name, int Year)
    {
        public Director ToDomainModel()
            => new(Name, Year.ToString());
    }
}
