using BHS.Contracts;
using BHS.Contracts.Blog;
using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace BHS.Infrastructure.Repositories.Mongo;

internal static class AggregateFluentExtensions
{
    /// <summary>
    /// Appends an aggregation pipeline which returns the current snapshot of each blog post.
    /// </summary>
    /// <remarks>
    /// This is the primary aggregation which makes the immutable design of <see cref="PostDto"/> possible.
    /// PostDto contains the complete history of all changes to a blog post,
    /// and this aggregation determines the latest revision, deletion, categories, etc. for each post.
    /// </remarks>
    public static IAggregateFluent<PostCurrentSnapshotDto> GetCurrentPostSnapshotDtos(this IAggregateFluent<PostDto> aggregateFluent, DateTimeOffset now)
        => aggregateFluent
            // Get LatestRevision and DateFirstPublished
            .Unwind<PostDto, PostUnwoundRevisionDto>(x => x.Revisions)
            .Unwind<PostUnwoundRevisionDto, PostUnwoundRevisionUnwoundPublicationDto>(x => x.Revisions.Publications)
            .Match(x => x.Revisions.Publications.DatePublished <= now)
            .SortBy(x => x.Revisions.Publications.DatePublished)
            .Group(x => x.Slug, x => new PostLatestRevisionDto(
                x.Key,
                x.Last().Revisions,
                x.First().Revisions.Publications.DatePublished,
                x.Last().Deletions,
                x.Last().Categories)
            )
            // Get latest Deletion
            .Unwind(x => x.Deletions, new AggregateUnwindOptions<PostLatestRevisionUnwoundDeletionDto>() { PreserveNullAndEmptyArrays = true })
            .Match(x => x.Deletions == null || x.Deletions.DateDeleted <= now)
            .SortBy(x => x.Deletions!.DateDeleted)
            .Group(x => x.Slug, x => new PostLatestRevisionUnwoundDeletionDto(
                x.Key,
                x.Last().LatestRevision,
                x.Last().DateFirstPublished,
                x.Last().Deletions,
                x.Last().Categories)
            )
            // Filter out any documents where DatePublished < DateDeleted.
            // We must use BsonDocument here because LINQ translation doesn't work.
            .Match(new BsonDocumentFilterDefinition<PostLatestRevisionUnwoundDeletionDto>(
                new BsonDocument(
                    "$expr",
                    new BsonDocument(
                        "$gt",
                        new BsonArray(2)
                        {
                            $"${nameof(PostLatestRevisionUnwoundDeletionDto.LatestRevision)}.{nameof(PostRevisionUnwoundPublicationDto.Publications)}.{nameof(PostRevisionPublicationDto.DatePublished)}",
                            $"${nameof(PostLatestRevisionUnwoundDeletionDto.Deletions)}.{nameof(PostDeletionDto.DateDeleted)}",
                        }
                    )
                )
            ))
            // Get enabled Categories
            .Unwind(x => x.Categories, new AggregateUnwindOptions<PostLatestRevisionNotDeletedUnwoundCategoryDto> { PreserveNullAndEmptyArrays = true })
            .Unwind(x => x.Categories!.Changes, new AggregateUnwindOptions<PostLatestRevisionNotDeletedUnwoundCategoryUnwoundChangeDto> { PreserveNullAndEmptyArrays = true })
            .Match(x => x.Categories == null || x.Categories.Changes.DateChanged <= now)
            .SortBy(x => x.Categories!.Changes.DateChanged)
            .Group(x => new PostFlattenedGroupedCategorySlugIdDto(x.Slug, x.Categories!.Slug), x => new PostLatestRevisionFlattenedGroupedCategoryDto(
                x.Key,
                x.Last().LatestRevision,
                x.Last().DateFirstPublished,
                x.Last().Categories))
            .Match(x => x.Categories == null || x.Categories.Changes.IsEnabled)
            // Re-combine unwound categories ONLY IF Category is not null (otherwise un-categorized posts get a null category).
            // We must use BsonDocument here because we need the $$REMOVE System Variable.
            .Group(new BsonDocumentProjectionDefinition<PostLatestRevisionFlattenedGroupedCategoryDto, PostCurrentSnapshotDto>(
                new BsonDocument
                {
                    { "_id", $"$_id.{nameof(PostFlattenedGroupedCategorySlugIdDto.PostSlug)}" },
                    { nameof(PostCurrentSnapshotDto.LatestRevision), new BsonDocument("$last", $"${nameof(PostLatestRevisionFlattenedGroupedCategoryDto.LatestRevision)}") },
                    { nameof(PostCurrentSnapshotDto.DateFirstPublished), new BsonDocument("$last", $"${nameof(PostLatestRevisionFlattenedGroupedCategoryDto.DateFirstPublished)}") },
                    { nameof(PostCurrentSnapshotDto.Categories),
                        new BsonDocument(
                            "$push",
                            new BsonDocument(
                                "$cond",
                                new BsonArray
                                {
                                    new BsonDocument(
                                        "$ne",
                                        new BsonArray
                                        {
                                            $"${nameof(PostLatestRevisionFlattenedGroupedCategoryDto.Categories)}",
                                            BsonNull.Value
                                        }),
                                    new BsonDocument
                                    {
                                        { "_id", $"${nameof(PostLatestRevisionFlattenedGroupedCategoryDto.Categories)}._id" },
                                        { nameof(PostCategoryDto.Name), $"${nameof(PostLatestRevisionFlattenedGroupedCategoryDto.Categories)}.{nameof(PostCategoryUnwoundChangeDto.Name)}" }
                                    },
                                    "$$REMOVE"
                                })) }
                })
            );

    [SuppressMessage("Performance", "CA1845:Use span-based 'string.Concat'", Justification = "CS8640:Expression tree cannot contain value of ref struct or restricted type 'ReadOnlySpan'.")]
    public static IAggregateFluent<PostPreview> GetPreviews(this IAggregateFluent<PostCurrentSnapshotDto> aggregateFluent, string? searchText = null)
    {
        // Need to use AppendStage instead of Project directly because we need to pass in TranslationOptions.

        if (searchText is null)
            return aggregateFluent
                .AppendStage(PipelineStageDefinitionBuilder.Project<PostCurrentSnapshotDto, PostPreview>(
                    x => new PostPreview(
                        x.Slug,
                        x.LatestRevision.Title,
                        x.LatestRevision.ContentMarkdown.Substring(0, 135) + "…",
                        x.LatestRevision.Author == null ? null : new Author(0, x.LatestRevision.Author.Username, x.LatestRevision.Author.DisplayName), // TODO: author id zero!
                        x.DateFirstPublished,
                        x.Categories.Select(x => new Category(x.Slug, x.Name))),
                    DbConstants.TranslationOptions)
                );
        else
            return aggregateFluent
                .Match(x => x.LatestRevision.ContentMarkdown.Contains(searchText))
                .AppendStage(PipelineStageDefinitionBuilder.Project<PostCurrentSnapshotDto, PostCurrentSnapshotWithSearchTextIdxDto>(
                    x => new PostCurrentSnapshotWithSearchTextIdxDto(
                        x.Slug,
                        x.LatestRevision,
                        x.DateFirstPublished,
                        x.Categories,
                        x.LatestRevision.ContentMarkdown.IndexOf(searchText) - 25),
                    DbConstants.TranslationOptions)
                )
                .AppendStage(PipelineStageDefinitionBuilder.Project<PostCurrentSnapshotWithSearchTextIdxDto, PostPreview>(
                    x => new PostPreview(
                        x.Slug,
                        x.LatestRevision.Title,
                        "…" + x.LatestRevision.ContentMarkdown.Substring(x.SearchTextHighlightStart, 135) + "…",
                        x.LatestRevision.Author == null ? null : new Author(0, x.LatestRevision.Author.Username, x.LatestRevision.Author.DisplayName), // TODO: author id zero!
                        x.DateFirstPublished,
                        x.Categories.Select(x => new Category(x.Slug, x.Name))),
                    DbConstants.TranslationOptions)
                );
    }
}
