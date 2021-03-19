using BHS.Contracts.Blog;

namespace BHS.DataAccess.Models
{
    public record CategoryDTO(
        string Slug,
        string Name,
        int PostsCount)
    {
        public Category ToDomainModel()
            => new(Slug, Name, PostsCount);
    }
}
