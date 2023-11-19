using BHS.Contracts;
using BHS.Contracts.Blog;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record PostDto(
    [property: BsonId] string Slug,
    IReadOnlyCollection<PostRevisionDto> Revisions,
    IReadOnlyCollection<PostDeletionDto> Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories)
{
    public static PostDto New(
        DateTimeOffset now,
        string slug,
        string title,
        string contentMarkdown,
        Uri? filePath,
        string? photosAlbumSlug,
        Author? author,
        DateTimeOffset datePublished,
        IEnumerable<Category>? categories)
        => new(
            Slug: slug,
            Revisions:
            [
                PostRevisionDto.New(
                    now,
                    title,
                    contentMarkdown,
                    filePath,
                    photosAlbumSlug,
                    author,
                    datePublished),
            ],
            Deletions: [],
            Categories: categories?.Select(category => PostCategoryHistoryDto.New(now, category)).ToArray() ?? []);
}

internal sealed record PostRevisionDto(
    ObjectId Id,
    DateTimeOffset DateRevised, // Server-set value. Decorative.
    string Title,
    string ContentMarkdown,
    string? FilePath,
    string? PhotosAlbumSlug,
    AuthorDto? Author,
    IReadOnlyCollection<PostRevisionPublicationDto> Publications)
{
    public static PostRevisionDto New(
        DateTimeOffset now,
        string title,
        string contentMarkdown,
        Uri? filePath,
        string? photosAlbumSlug,
        Author? author,
        DateTimeOffset datePublished)
        => new(
            ObjectId.GenerateNewId(now.UtcDateTime),
            now,
            title,
            contentMarkdown,
            filePath?.ToString(),
            photosAlbumSlug,
            AuthorDto.FromAuthor(author),
            [
                PostRevisionPublicationDto.New( // TODO: consider not immediately publishing a new revision?
                    datePublished,
                    now),
            ]);
}

internal sealed record PostRevisionPublicationDto(
    ObjectId Id,
    DateTimeOffset DatePublished, // User-set value. Determines final visibility of the post.
    DateTimeOffset DateCommitted) // Server-set value. Determines which publication is latest, if past latest deletion.
{
    public static PostRevisionPublicationDto New(DateTimeOffset datePublished, DateTimeOffset now)
        => new(ObjectId.GenerateNewId(now.UtcDateTime), datePublished, now);
}

internal sealed record PostDeletionDto(
    ObjectId Id,
    DateTimeOffset DateDeleted) // Server-set value. Determines the cutoff for which publication commit dates will not be evaluated.
{
    public static PostDeletionDto New(DateTimeOffset now)
        => new(ObjectId.GenerateNewId(now.UtcDateTime), now);
}

internal record PostCategoryDto(
    [property: BsonId] string Slug,
    string Name)
{
    public Category ToCategory()
        => new(Slug, Name);
}

internal sealed record PostCategoryHistoryDto(
    string Slug,
    string Name,
    IReadOnlyCollection<PostCategoryChangeDto> Changes) : PostCategoryDto(Slug, Name)
{
    public static PostCategoryHistoryDto New(DateTimeOffset now, Category category)
        => new(
            category.Slug,
            category.Name,
            [
                new PostCategoryChangeDto(
                    ObjectId.GenerateNewId(now.UtcDateTime),
                    now,
                    true),
            ]);
}

internal sealed record PostCategoryChangeDto(
    ObjectId Id,
    DateTimeOffset DateChanged, // Server-set value. Determines which category change is latest.
    bool IsEnabled)
{
    public static PostCategoryChangeDto New(DateTimeOffset now, bool IsEnabled)
        => new(ObjectId.GenerateNewId(now.UtcDateTime), now, IsEnabled);
}


internal sealed record PostUnwoundCategoryDto(
    [property: BsonId] string Slug,
    IReadOnlyCollection<PostRevisionDto> Revisions,
    IReadOnlyCollection<PostDeletionDto> Deletions,
    PostCategoryHistoryDto Categories);


internal sealed record PostUnwoundRevisionDto(
    [property: BsonId] string Slug,
    PostRevisionDto Revisions,
    IReadOnlyCollection<PostDeletionDto> Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories);

internal sealed record PostUnwoundRevisionUnwoundPublicationDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto Revisions,
    IReadOnlyCollection<PostDeletionDto> Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories);

internal sealed record PostRevisionUnwoundPublicationDto(
    ObjectId Id,
    DateTimeOffset DateRevised,
    string Title,
    string ContentMarkdown,
    string? FilePath,
    string? PhotosAlbumSlug,
    AuthorDto? Author,
    PostRevisionPublicationDto Publications);

internal sealed record PostLatestRevisionDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    PostRevisionPublicationDto LatestPublication,
    IReadOnlyCollection<PostDeletionDto> Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories);

internal sealed record PostLatestRevisionUnwoundDeletionDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    PostRevisionPublicationDto LatestPublication,
    PostDeletionDto? Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories);

internal sealed record PostLatestRevisionNotDeletedUnwoundCategoryDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    PostRevisionPublicationDto LatestPublication,
    PostCategoryHistoryDto? Categories);

internal sealed record PostLatestRevisionNotDeletedUnwoundCategoryUnwoundChangeDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    PostRevisionPublicationDto LatestPublication,
    PostCategoryUnwoundChangeDto? Categories);

internal sealed record PostCategoryUnwoundChangeDto(
    [property: BsonId] string Slug,
    string Name,
    PostCategoryChangeDto Changes);

internal sealed record PostLatestRevisionFlattenedGroupedCategoryDto(
    [property: BsonId] PostFlattenedGroupedCategorySlugIdDto Id,
    PostRevisionUnwoundPublicationDto LatestRevision,
    PostRevisionPublicationDto LatestPublication,
    PostCategoryUnwoundChangeDto? Categories);

internal sealed record PostFlattenedGroupedCategorySlugIdDto(
    string PostSlug,
    string? CategorySlug);

internal sealed record PostCurrentSnapshotDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    PostRevisionPublicationDto LatestPublication,
    IEnumerable<PostCategoryDto> Categories)
{
    public Post ToPost()
        => new(
            Slug,
            LatestRevision.Title,
            LatestRevision.ContentMarkdown,
            LatestRevision.FilePath is null ? null : new Uri(LatestRevision.FilePath),
            LatestRevision.PhotosAlbumSlug,
            LatestRevision.Author?.ToAuthor(),
            LatestPublication.DatePublished,
            LatestPublication.DateCommitted,
            Categories.Select(x => x.ToCategory()).ToList());
}

internal sealed record PostCurrentSnapshotWithSearchTextIdxDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    PostRevisionPublicationDto LatestPublication,
    IEnumerable<PostCategoryDto> Categories,
    int SearchTextHighlightStart);
