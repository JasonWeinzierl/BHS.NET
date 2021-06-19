namespace BHS.Contracts.Blog
{
    public record CategorySummary(
        string Slug,
        string Name,
        int PostsCount)
        : Category(Slug, Name);
}
