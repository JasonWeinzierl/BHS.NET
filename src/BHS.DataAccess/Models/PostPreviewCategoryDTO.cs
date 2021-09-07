using BHS.Contracts;
using BHS.Contracts.Blog;

namespace BHS.DataAccess.Models
{
    public record PostPreviewCategoryDto(
        string Slug,
        string Title,
        string ContentPreview,

        int? AuthorId,
        string? AuthorDisplayName,
        string? AuthorName,

        string CategorySlug,
        string CategoryName,

        DateTimeOffset DatePublished)
    {
        public Category ToCategory()
            => new(CategorySlug, CategoryName);

        public static PostPreview ToDomainModel(IEnumerable<PostPreviewCategoryDto> rows)
        {
            var post = rows.First();

            Author? author = null;
            if (post.AuthorId.HasValue && post.AuthorDisplayName is not null)
                author = new Author(post.AuthorId.Value, post.AuthorDisplayName, post.AuthorName);

            return new(post.Slug,
                post.Title,
                post.ContentPreview,
                author,
                post.DatePublished,
                rows.Select(p => p.ToCategory()).Where(c => c.Slug is not null).ToList());
        }
    }
}
