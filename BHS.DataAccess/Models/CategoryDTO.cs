using BHS.Contracts.Blog;
using System.Collections.Generic;

namespace BHS.DataAccess.Models
{
    public record CategoryDTO(
        string Slug,
        string Name,
        int PostsCount)
    {
        public Category ToDomainModel()
            => new(Slug, Name, PostsCount);

        public CategoryPosts ToDomainModel(IEnumerable<PostPreview> posts)
            => new(Slug, Name, PostsCount, posts);
    }
}
