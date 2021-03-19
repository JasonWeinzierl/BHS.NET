namespace BHS.Contracts.Blog
{
    public record Category(
        string Slug,
        string Name,
        int PostsCount);
}
