﻿using BHS.Contracts.Photos;

namespace BHS.Domain.Photos;

public interface IAlbumRepository
{
    Task<AlbumPhotos?> GetBySlug(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Album>> GetAll(CancellationToken cancellationToken = default);
    Task<Photo?> GetPhoto(string albumSlug, string photoId, CancellationToken cancellationToken = default);
}
