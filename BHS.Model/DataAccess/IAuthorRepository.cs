﻿using BHS.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByUserName(string userName);
        Task<IEnumerable<Author>> GetAll();
    }
}