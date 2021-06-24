using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.Model.DataAccess
{
    public interface IPostPreviewRepository
    {
        Task<IReadOnlyCollection<PostPreview>> GetByAuthorId(int authorId);
        Task<IReadOnlyCollection<PostPreview>> GetByCategorySlug(string categorySlug);
        Task<IReadOnlyCollection<PostPreview>> Search(string? text, DateTimeOffset? from, DateTimeOffset? to);
    }
}
