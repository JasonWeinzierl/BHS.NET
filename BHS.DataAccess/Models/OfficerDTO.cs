using BHS.Contracts.Leadership;

namespace BHS.DataAccess.Models
{
    public record OfficerDto(string Title, string Name, int SortOrder)
    {
        public Officer ToDomainModel()
            => new(Title, Name);
    }
}
