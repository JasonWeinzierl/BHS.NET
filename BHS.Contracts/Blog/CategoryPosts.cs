using System.Collections.Generic;

namespace BHS.Contracts.Blog
{
    public record CategoryPosts(
        string Slug,
        string Name,
        int PostsCount,
        IEnumerable<PostPreview> Posts)
        : Category(Slug, Name, PostsCount);
}
