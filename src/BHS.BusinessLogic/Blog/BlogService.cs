using BHS.Contracts.Blog;
using BHS.Model.DataAccess;
using BHS.Model.Exceptions;
using BHS.Model.Services.Blog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHS.BusinessLogic.Blog
{
    public class BlogService : IBlogService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostPreviewRepository _postPreviewRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger _logger;

        public BlogService(
            IPostRepository postRepository,
            IPostPreviewRepository postPreviewRepository,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository,
            ILogger<BlogService> logger)
        {
            _postRepository = postRepository;
            _postPreviewRepository = postPreviewRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _logger = logger;
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
        {
            return _postRepository.GetBySlug(slug);
        }

        public Task<IReadOnlyCollection<CategorySummary>> GetCategories()
        {
            return _categoryRepository.GetAll();
        }

        public async Task<IReadOnlyCollection<PostPreview>> GetPostsByAuthor(string username)
        {
            var author = await _authorRepository.GetByUserName(username);
            if (author is null)
                throw new NotFoundException($"Author '{username}' does not exist.");

            return await _postPreviewRepository.GetByAuthorId(author.Id);
        }

        public Task<IReadOnlyCollection<PostPreview>> SearchPosts(string? text, DateTime? from, DateTime? to)
        {
            return _postPreviewRepository.Search(text, from, to);
        }
    }
}
