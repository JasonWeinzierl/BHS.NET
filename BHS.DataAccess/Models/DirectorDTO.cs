using BHS.Contracts.Leadership;
using System;

namespace BHS.DataAccess.Models
{
    public record DirectorDTO(string Name, int Year)
    {
        public Director ToDomainModel()
            => new(Name, Year.ToString());
    }
}
