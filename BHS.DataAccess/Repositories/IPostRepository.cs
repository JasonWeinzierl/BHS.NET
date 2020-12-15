using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetBySlug(string slug);
    }
}
