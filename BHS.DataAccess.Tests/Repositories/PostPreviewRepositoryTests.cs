using BHS.Contracts.Blog;
using BHS.DataAccess.Core;
using BHS.DataAccess.Tests;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BHS.DataAccess.Repositories.Tests
{
    public class PostPreviewRepositoryTests
    {
        private readonly PostPreviewRepository Subject;

        private readonly MockDataSource MockData = new MockDataSource();

        public PostPreviewRepositoryTests()
        {
            Subject = new PostPreviewRepository(new Querier(MockData.CreateDbConnectionFactory().Object));
        }

        [Fact]
        public async Task Search_FillsResult()
        {
            var postPreview = new PostPreview("b", "c", "d", 1, new DateTimeOffset(2020, 12, 15, 22, 39, 0, TimeSpan.FromHours(-6)));
            MockData.SetReaderResultset(new PostPreview[] { postPreview });

            var result = await Subject.Search("a", default, default).FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.Equal(postPreview.Slug, result.Slug);
            Assert.Equal(postPreview.Title, result.Title);
            Assert.Equal(postPreview.ContentPreview, result.ContentPreview);
            Assert.Equal(postPreview.AuthorId, result.AuthorId);
            Assert.Equal(postPreview.DatePublished, result.DatePublished);
        }

        [Fact]
        public async Task Search_Command()
        {
            MockData.ReaderResultset = new DataTable();
            var start = new DateTimeOffset(2020, 12, 14, 20, 43, 0, TimeSpan.FromHours(-6));
            var end = new DateTimeOffset(2020, 12, 14, 20, 44, 0, TimeSpan.FromHours(-6));

            _ = await Subject.Search("a", start, end).ToListAsync();

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("blog.PostPreview_Search", MockData.CommandText);

            Assert.Equal("@searchText", MockData.Parameters[0].ParameterName);
            Assert.Equal("a", MockData.Parameters[0].Value);
            Assert.Equal("@fromDate", MockData.Parameters[1].ParameterName);
            Assert.Equal(start, MockData.Parameters[1].Value);
            Assert.Equal("@toDate", MockData.Parameters[2].ParameterName);
            Assert.Equal(end, MockData.Parameters[2].Value);
        }

        [Fact]
        public async Task GetByCategorySlug_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetByCategorySlug("a").ToListAsync();

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("blog.PostPreview_GetByCategorySlug", MockData.CommandText);

            Assert.Equal("@categorySlug", MockData.Parameters[0].ParameterName);
            Assert.Equal("a", MockData.Parameters[0].Value);
        }

        [Fact]
        public async Task GetByAuthorId_Command()
        {
            MockData.ReaderResultset = new DataTable();

            _ = await Subject.GetByAuthorId(1).ToListAsync();

            Assert.Equal(Constants.bhsConnectionStringName, MockData.ConnectionStringName);
            Assert.Equal("blog.PostPreview_GetByAuthorId", MockData.CommandText);

            Assert.Equal("@authorId", MockData.Parameters[0].ParameterName);
            Assert.Equal(1, MockData.Parameters[0].Value);
        }
    }
}
