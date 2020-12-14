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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger _logger;

        public BlogService(
            IPostRepository postRepository,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository,
            ILogger<BlogService> logger)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public Task<Category> GetCategory(string slug)
        {
            return _categoryRepository.GetBySlug(slug);
        }

        public Task<IEnumerable<Post>> GetPostsByCategory(string slug)
        {
            return _postRepository.GetByCategorySlug(slug);
        }

        public Task<Post> GetPost(string slug)
        {
            return _postRepository.GetBySlug(slug);
        }

        public Task<IEnumerable<Category>> GetCategoriesByPost(string slug)
        {
            return _categoryRepository.GetByPostSlug(slug);
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthor(string username)
        {
            var author = await _authorRepository.GetByUserName(username);

            return author == default ? default : await _postRepository.GetByAuthorId(author.Id);
        }

        public Task<IEnumerable<Post>> SearchPosts(string text, DateTime? from, DateTime? to)
        {
            return _postRepository.Search(text, from, to);
        }
    }
}
