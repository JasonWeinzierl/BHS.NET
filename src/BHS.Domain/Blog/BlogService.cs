using BHS.Contracts.Blog;
using BHS.Domain.Authors;

namespace BHS.Domain.Blog;

public class BlogService : IBlogService
{
    private readonly IPostRepository _postRepository;
    private readonly IPostPreviewRepository _postPreviewRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAuthorRepository _authorRepository;

    public BlogService(
        IPostRepository postRepository,
        IPostPreviewRepository postPreviewRepository,
        ICategoryRepository categoryRepository,
        IAuthorRepository authorRepository)
    {
        _postRepository = postRepository;
        _postPreviewRepository = postPreviewRepository;
        _categoryRepository = categoryRepository;
        _authorRepository = authorRepository;
    }

    public async Task<CategoryPosts?> GetCategory(string slug, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetBySlug(slug, cancellationToken);
        if (category is null)
            return null;

        var posts = await _postPreviewRepository.GetByCategorySlug(slug, cancellationToken);

        return new CategoryPosts(category.Slug, category.Name, posts);
    }

    public Task<Post?> GetPost(string slug, CancellationToken cancellationToken = default)
        => _postRepository.GetBySlug(slug, cancellationToken);

    public Task<IReadOnlyCollection<CategorySummary>> GetCategories(CancellationToken cancellationToken = default)
        => _categoryRepository.GetAll(cancellationToken);

    public async Task<IReadOnlyCollection<PostPreview>?> GetPostsByAuthor(string username, CancellationToken cancellationToken = default)
    {
        var author = await _authorRepository.GetByUserName(username, cancellationToken);
        if (author is null)
            return null;

        return await _postPreviewRepository.GetByAuthorId(author.Id, cancellationToken);
    }

    public Task<IReadOnlyCollection<PostPreview>> SearchPosts(string? text, DateTime? from, DateTime? to, CancellationToken cancellationToken = default)
        => _postPreviewRepository.Search(text, from, to, cancellationToken);
}
