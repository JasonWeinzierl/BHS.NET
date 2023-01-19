using BHS.Contracts.Leadership;

namespace BHS.Infrastructure.Repositories.Sql.Models;

public record DirectorDto(string Name, int Year)
{
    public Director ToDomainModel()
        => new(Name, Year.ToString());
}
