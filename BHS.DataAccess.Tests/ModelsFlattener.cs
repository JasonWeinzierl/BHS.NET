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

            table.Columns.Add(nameof(Post.Id), typeof(int));
            table.Columns.Add(nameof(Post.Title), typeof(string));
            table.Columns.Add(nameof(Post.BodyContent), typeof(string));
            table.Columns.Add(nameof(Post.FilePath), typeof(string));
            table.Columns.Add(nameof(Post.PhotosAlbumId), typeof(int));
            table.Columns.Add(nameof(Post.IsVisible), typeof(bool));
            table.Columns.Add(nameof(Post.AuthorId), typeof(int));
            table.Columns.Add(nameof(Post.PublishDate), typeof(DateTimeOffset));

            foreach (var post in posts)
            {
                table.Rows.Add(post.Id, post.Title, post.BodyContent, post.FilePath, post.PhotosAlbumId, post.IsVisible, post.AuthorId, post.PublishDate);
            }

            return table;
        }
    }
}
