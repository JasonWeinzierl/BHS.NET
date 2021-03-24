using BHS.Contracts.Blog;

namespace BHS.DataAccess.Models
{
    public record CategoryDTO(
        string Slug,
        string Name)
    {
        public Category ToDomainModel()
            => new(Slug, Name);
    }
}
