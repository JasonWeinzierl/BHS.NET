using BHS.Contracts.Blog;
using System;
using System.Collections.Generic;
using System.Data;

namespace BHS.DataAccess.Tests
{
    public static class ModelsFlattener
    {
        public static DataTable ToDataTable(IEnumerable<Post> posts)
        {
            var table = new DataTable();

            table.Columns.Add(nameof(Post.Slug), typeof(string));
            table.Columns.Add(nameof(Post.Title), typeof(string));
            table.Columns.Add(nameof(Post.ContentMarkdown), typeof(string));
            table.Columns.Add(nameof(Post.FilePath), typeof(string));
            table.Columns.Add(nameof(Post.PhotosAlbumId), typeof(int));
            table.Columns.Add(nameof(Post.AuthorId), typeof(int));
            table.Columns.Add(nameof(Post.DatePublished), typeof(DateTimeOffset));
            table.Columns.Add(nameof(Post.DateLastModified), typeof(DateTimeOffset));

            foreach (var post in posts)
            {
                table.Rows.Add(post.Slug, post.Title, post.ContentMarkdown, post.FilePath?.ToString(), post.PhotosAlbumId, post.AuthorId, post.DatePublished, post.DateLastModified);
            }

            return table;
        }
    }
}
