using BHS.Contracts.Blog;
using BHS.DataAccess.Tests;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PostPreviewRepositoryTests
    {
        private readonly PostPreviewRepository Subject;

        private readonly MockQuerier MockQuerier = new MockQuerier();

        public PostPreviewRepositoryTests()
        {
            Subject = new PostPreviewRepository(MockQuerier);
        }

        [Fact]
        public async Task Search_Executes()
        {
            MockQuerier.ManyResults = new PostPreview[]
            {
                new PostPreview("b", "c", "d", 1, new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
            };
            var start = new DateTimeOffset(2020, 12, 14, 20, 43, 0, TimeSpan.FromHours(-6));
            var end = new DateTimeOffset(2020, 12, 14, 20, 44, 0, TimeSpan.FromHours(-6));

            _ = await Subject.Search("a", start, end);

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("blog.PostPreview_Search", MockQuerier.CommandText);

            Assert.Equal("a", MockQuerier.Parameters.searchText);
            Assert.Equal(start, MockQuerier.Parameters.fromDate);
            Assert.Equal(end, MockQuerier.Parameters.toDate);
        }

        [Fact]
        public async Task GetByCategorySlug_Executes()
        {
            MockQuerier.ManyResults = new PostPreview[]
            {
                new PostPreview("b", "c", "d", 1, new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
            };

            _ = await Subject.GetByCategorySlug("a");

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("blog.PostPreview_GetByCategorySlug", MockQuerier.CommandText);

            Assert.Equal("a", MockQuerier.Parameters.categorySlug);
        }

        [Fact]
        public async Task GetByAuthorId_Executes()
        {
            MockQuerier.ManyResults = new PostPreview[]
            {
                new PostPreview("b", "c", "d", 1, new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)))
            };

            _ = await Subject.GetByAuthorId(1);

            Assert.Equal(Constants.bhsConnectionStringName, MockQuerier.ConnectionStringName);
            Assert.Equal("blog.PostPreview_GetByAuthorId", MockQuerier.CommandText);

            Assert.Equal(1, MockQuerier.Parameters.authorId);
        }
    }
}
