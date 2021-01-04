using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.DataAccess.Repositories
{
    public interface IPostPreviewRepository
    {
        Task<IEnumerable<PostPreview>> GetByAuthorId(int authorId);
        Task<IEnumerable<PostPreview>> GetByCategorySlug(string categorySlug);
        Task<IEnumerable<PostPreview>> Search(string text, DateTimeOffset? from, DateTimeOffset? to);
    }
}
