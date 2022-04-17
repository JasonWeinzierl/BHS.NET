using BHS.Contracts.Blog;

namespace BHS.Infrastructure.Models
{
    public record CategorySummaryDto(
        string Slug,
        string Name,
        int PostsCount)
    {
        public CategorySummary ToDomainModel()
            => new(Slug, Name, PostsCount);
    }
}
