using BHS.Contracts.Blog;
using BHS.Domain.Authors;

namespace BHS.Domain.Blog
{
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

        public async Task<CategoryPosts?> GetCategory(string slug)
        {
            var category = await _categoryRepository.GetBySlug(slug);
            if (category is null)
                return null;

            var posts = await _postPreviewRepository.GetByCategorySlug(slug);

            return new CategoryPosts(category.Slug, category.Name, posts);
        }

        public Task<Post?> GetPost(string slug)
            => _postRepository.GetBySlug(slug);

        public Task<IReadOnlyCollection<CategorySummary>> GetCategories()
            => _categoryRepository.GetAll();

        public async Task<IReadOnlyCollection<PostPreview>?> GetPostsByAuthor(string username)
        {
            var author = await _authorRepository.GetByUserName(username);
            if (author is null)
                return null;

            return await _postPreviewRepository.GetByAuthorId(author.Id);
        }

        public Task<IReadOnlyCollection<PostPreview>> SearchPosts(string? text, DateTime? from, DateTime? to)
            => _postPreviewRepository.Search(text, from, to);
    }
}
