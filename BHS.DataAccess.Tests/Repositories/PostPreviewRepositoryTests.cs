using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PostPreviewRepositoryTests
    {
        private readonly PostPreviewRepository _subject;

        private readonly MockExecuter _mockExecuter = new();

        public PostPreviewRepositoryTests()
        {
            _subject = new PostPreviewRepository(_mockExecuter);
        }

        [Fact]
        public async Task Search_Executes()
        {
            _mockExecuter.ManyResults = new PostPreview[]
            {
                new PostPreview("b", "c", "d", 1, new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
            };
            var start = new DateTimeOffset(2020, 12, 14, 20, 43, 0, TimeSpan.FromHours(-6));
            var end = new DateTimeOffset(2020, 12, 14, 20, 44, 0, TimeSpan.FromHours(-6));

            _ = await _subject.Search("a", start, end);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.PostPreview_Search", _mockExecuter.CommandText);

            Assert.Equal("a", _mockExecuter.Parameters?.searchText);
            Assert.Equal(start, _mockExecuter.Parameters?.fromDate);
            Assert.Equal(end, _mockExecuter.Parameters?.toDate);
        }

        [Fact]
        public async Task GetByCategorySlug_Executes()
        {
            _mockExecuter.ManyResults = new PostPreview[]
            {
                new PostPreview("b", "c", "d", 1, new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
            };

            _ = await _subject.GetByCategorySlug("a");

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.PostPreview_GetByCategorySlug", _mockExecuter.CommandText);

            Assert.Equal("a", _mockExecuter.Parameters?.categorySlug);
        }

        [Fact]
        public async Task GetByAuthorId_Executes()
        {
            _mockExecuter.ManyResults = new PostPreview[]
            {
                new PostPreview("b", "c", "d", 1, new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
            };

            _ = await _subject.GetByAuthorId(1);

            Assert.Equal(Constants.bhsConnectionStringName, _mockExecuter.ConnectionStringName);
            Assert.Equal("blog.PostPreview_GetByAuthorId", _mockExecuter.CommandText);

            Assert.Equal(1, _mockExecuter.Parameters?.authorId);
        }
    }
}
