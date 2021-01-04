using BHS.Contracts.Blog;
using BHS.DataAccess.Repositories;
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

        public Task<Category> GetCategory(string slug)
        {
            return _categoryRepository.GetBySlug(slug);
        }

        public Task<IEnumerable<PostPreview>> GetPostsByCategory(string slug)
        {
            return _postPreviewRepository.GetByCategorySlug(slug);
        }

        public Task<Post> GetPost(string slug)
        {
            return _postRepository.GetBySlug(slug);
        }

        public Task<IEnumerable<Category>> GetCategoriesByPost(string slug)
        {
            return _categoryRepository.GetByPostSlug(slug);
        }

        public async Task<IEnumerable<PostPreview>> GetPostsByAuthor(string username)
        {
            var author = await _authorRepository.GetByUserName(username);
            if (author == default)
                return default;

            return await _postPreviewRepository.GetByAuthorId(author.Id);
        }

        public Task<IEnumerable<PostPreview>> SearchPosts(string text, DateTime? from, DateTime? to)
        {
            return _postPreviewRepository.Search(text, from, to);
        }
    }
}
