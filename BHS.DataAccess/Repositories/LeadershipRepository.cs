﻿using BHS.Contracts.Leadership;
using BHS.DataAccess.Core;
using BHS.DataAccess.Models;
using BHS.Model.DataAccess;
using BHS.Model.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public class LeadershipRepository : ILeadershipRepository
    {
        protected IDbExecuter E { get; }

        private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

        public LeadershipRepository(
            IDbExecuter executer,
            IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            E = executer;
            _dateTimeOffsetProvider = dateTimeOffsetProvider;
        }

        public async Task<IList<Officer>> GetCurrentOfficers()
        {
            var results = await E.QueryAsync<OfficerDTO>(Constants.bhsConnectionStringName, "leadership.Officer_GetAll");
            return results.OrderBy(r => r.SortOrder).Select(r => r.ToDomainModel()).ToList();
        }

        public async Task<IList<Director>> GetCurrentDirectors()
        {
            var results = await E.QueryAsync<DirectorDTO>(Constants.bhsConnectionStringName, "leadership.Director_GetCurrent", new
            {
                startingYear = _dateTimeOffsetProvider.CurrentYear()
            });
            return results.Select(r => r.ToDomainModel()).ToList();
        }
    }
}