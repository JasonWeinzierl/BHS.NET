using System;

namespace BHS.Contracts.Blog
{
    // todo: implement post hook functionality, probably in the sproc
    // that pulls back a 135 character hook.  markdown entities
    // should be stripped except newlines.  search text should be in
    // center of hook if applicable and marked.  ellipses should be on
    // bounds where relevant.
    public record PostPreview(
        string Slug,
        string Title,
        string ContentPreview,
        int AuthorId,
        DateTimeOffset DatePublished);
}
