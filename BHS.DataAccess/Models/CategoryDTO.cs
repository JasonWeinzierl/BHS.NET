using BHS.Contracts.Blog;
using System.Collections.Generic;

namespace BHS.DataAccess.Models
{
    public record CategoryDTO(
        string Slug,
        string Name)
    {
        public Category ToDomainModel()
            => new(Slug, Name);

        public CategoryPosts ToDomainModel(IEnumerable<PostPreview> posts)
            => new(Slug, Name, posts);
    }
}
