﻿using BHS.Contracts;
using BHS.Contracts.Blog;
using MongoDB.Bson.Serialization.Attributes;

namespace BHS.Infrastructure.Repositories.Mongo.Models;

internal sealed record PostDto(
    [property: BsonId] string Slug,
    IReadOnlyCollection<PostRevisionDto> Revisions,
    IReadOnlyCollection<PostDeletionDto> Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories)
{
    public static PostDto NewDraft(DateTimeOffset now, string slug, string title, string contentMarkdown, string? filePath, string? photosAlbumSlug, Author? author)
        => new(
            Slug: slug,
            Revisions: new[] { new PostRevisionDto(now, title, contentMarkdown, filePath, photosAlbumSlug, AuthorDto.FromAuthor(author), Array.Empty<PostRevisionPublicationDto>()) },
            Deletions: Array.Empty<PostDeletionDto>(),
            Categories: Array.Empty<PostCategoryHistoryDto>());
}

internal sealed record PostRevisionDto(
    DateTimeOffset DateRevised,
    string Title,
    string ContentMarkdown,
    string? FilePath,
    string? PhotosAlbumSlug,
    AuthorDto? Author,
    IReadOnlyCollection<PostRevisionPublicationDto> Publications);

internal sealed record PostRevisionPublicationDto(
    DateTimeOffset DatePublished);

internal sealed record PostDeletionDto(
    DateTimeOffset DateDeleted);

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
    IReadOnlyCollection<PostCategoryChangeDto> Changes) : PostCategoryDto(Slug, Name);

internal sealed record PostCategoryChangeDto(
    DateTimeOffset DateChanged,
    bool IsEnabled);


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
    DateTimeOffset DateFirstPublished,
    IReadOnlyCollection<PostDeletionDto> Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories);

internal sealed record PostLatestRevisionUnwoundDeletionDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    DateTimeOffset DateFirstPublished,
    PostDeletionDto? Deletions,
    IReadOnlyCollection<PostCategoryHistoryDto> Categories);

internal sealed record PostLatestRevisionNotDeletedUnwoundCategoryDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    DateTimeOffset DateFirstPublished,
    PostCategoryHistoryDto? Categories);

internal sealed record PostLatestRevisionNotDeletedUnwoundCategoryUnwoundChangeDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    DateTimeOffset DateFirstPublished,
    PostCategoryUnwoundChangeDto? Categories);

internal sealed record PostCategoryUnwoundChangeDto(
    [property: BsonId] string Slug,
    string Name,
    PostCategoryChangeDto Changes);

internal sealed record PostLatestRevisionFlattenedGroupedCategoryDto(
    [property: BsonId] PostFlattenedGroupedCategorySlugIdDto Id,
    PostRevisionUnwoundPublicationDto LatestRevision,
    DateTimeOffset DateFirstPublished,
    PostCategoryUnwoundChangeDto? Categories);

internal sealed record PostFlattenedGroupedCategorySlugIdDto(
    string PostSlug,
    string? CategorySlug);

internal sealed record PostCurrentSnapshotDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    DateTimeOffset DateFirstPublished,
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
            DateFirstPublished,
            LatestRevision.Publications.DatePublished,
            Categories.Select(x => x.ToCategory()).ToList());
}

internal sealed record PostCurrentSnapshotWithSearchTextIdxDto(
    [property: BsonId] string Slug,
    PostRevisionUnwoundPublicationDto LatestRevision,
    DateTimeOffset DateFirstPublished,
    IEnumerable<PostCategoryDto> Categories,
    int SearchTextHighlightStart);