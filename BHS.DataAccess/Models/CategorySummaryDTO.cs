using BHS.Contracts.Blog;

namespace BHS.DataAccess.Models
{
    public record CategorySummaryDTO(
        string Slug,
        string Name,
        int PostsCount)
    {
        public CategorySummary ToDomainModel()
            => new(Slug, Name, PostsCount);
    }
}
