﻿using BHS.Contracts.Leadership;

namespace BHS.Infrastructure.Models
{
    public record OfficerDto(string Title, string Name, int SortOrder)
    {
        public Officer ToDomainModel()
            => new(Title, Name);
    }
}