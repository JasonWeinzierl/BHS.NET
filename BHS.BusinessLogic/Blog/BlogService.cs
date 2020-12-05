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

        public Task<Category> GetCategory(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Task<IEnumerable<Post>> GetPostsByCategory(int id)
        {
            return _postRepository.GetByCategoryId(id);
        }

        public Task<Post> GetPost(int id)
        {
            return _postRepository.GetById(id);
        }

        public Task<IEnumerable<Category>> GetCategoriesByPost(int id)
        {
            return _categoryRepository.GetByPostId(id);
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
