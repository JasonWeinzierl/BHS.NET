using BHS.Contracts.Leadership;

namespace BHS.Infrastructure.Models;

public record DirectorDto(string Name, int Year)
{
    public Director ToDomainModel()
        => new(Name, Year.ToString());
}
