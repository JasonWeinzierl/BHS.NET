using BHS.Contracts.Blog;

namespace BHS.DataAccess.Models
{
    public record CategoryDto(
        string Slug,
        string Name)
    {
        public Category ToDomainModel()
            => new(Slug, Name);
    }
}
