using BHS.Infrastructure.Repositories.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BHS.Infrastructure.Repositories.Mongo;

internal static class AggregateFluentExtensions
{
    /// <summary>
    /// Appends an aggregation pipeline which contains the current snapshot of each blog post.
    /// </summary>
    /// <remarks>
    /// This is the primary aggregation which makes the immutable design of <see cref="PostDto"/> possible.
    /// PostDto contains the complete history of all changes to a blog post,
    /// and this aggregation determines the latest revision, deletion, categories, etc. for each post.
    /// </remarks>
    public static IAggregateFluent<PostCurrentSnapshotDto> GetCurrentPostSnapshots(this IAggregateFluent<PostDto> aggregateFluent, DateTimeOffset now)
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
            .Unwind<PostLatestRevisionDto, PostLatestRevisionUnwoundDeletionDto>(x => x.Deletions)
            .Match(x => x.Deletions.DateDeleted <= now)
            .SortBy(x => x.Deletions.DateDeleted)
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
            .Unwind<PostLatestRevisionUnwoundDeletionDto, PostLatestRevisionLatestDeletionUnwoundCategoryDto>(x => x.Categories)
            .Unwind<PostLatestRevisionLatestDeletionUnwoundCategoryDto, PostLatestRevisionLatestDeletionUnwoundCategoryUnwoundChangeDto>(x => x.Categories.Changes)
            .Match(x => x.Categories.Changes.DateChanged <= now)
            .SortBy(x => x.Categories.Changes.DateChanged)
            .Group(x => new { PostSlug = x.Slug, CategorySlug = x.Categories.Slug }, x => new
            {
                x.Key,
                x.Last().LatestRevision,
                x.Last().DateFirstPublished,
                x.Last().LatestDeletion,
                x.Last().Categories,
            })
            .Match(x => x.Categories.Changes.IsEnabled)
            .Group(x => x.Key.PostSlug, x => new PostCurrentSnapshotDto(
                x.Key,
                x.Last().LatestRevision,
                x.Last().DateFirstPublished,
                x.Last().LatestDeletion,
                x.Select(y => new PostCategoryDto(y.Categories.Slug, y.Categories.Name)))
            );
}
