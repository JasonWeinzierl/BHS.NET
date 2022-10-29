namespace BHS.Contracts.Blog;

public record CategoryPosts(
    string Slug,
    string Name,
    IEnumerable<PostPreview> Posts)
    : Category(Slug, Name);
