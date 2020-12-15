using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;

namespace BHS.DataAccess.Repositories
{
    public interface IPostPreviewRepository
    {
        IAsyncEnumerable<PostPreview> GetByAuthorId(int authorId);
        IAsyncEnumerable<PostPreview> GetByCategorySlug(string categorySlug);
        IAsyncEnumerable<PostPreview> Search(string text, DateTimeOffset? from, DateTimeOffset? to);
    }
}
