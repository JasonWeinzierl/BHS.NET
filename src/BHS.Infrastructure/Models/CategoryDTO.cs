using BHS.Contracts.Blog;

namespace BHS.Infrastructure.Models
{
    public record CategoryDto(
        string Slug,
        string Name)
    {
        public Category ToDomainModel()
            => new(Slug, Name);
    }
}
