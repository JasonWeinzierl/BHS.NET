﻿using BHS.Contracts.Photos;
using BHS.Domain.Photos;
using BHS.Infrastructure.Core;
using BHS.Infrastructure.Repositories.Sql.Models;

namespace BHS.Infrastructure.Repositories.Sql;

public class AlbumRepository : IAlbumRepository
{
    protected IDbExecuter E { get; }

    public AlbumRepository(IDbExecuter executer)
    {
        E = executer;
    }

    public async Task<AlbumPhotos?> GetBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var (albums, photos) = await E.ExecuteSprocQueryMultiple<AlbumDto, Photo>(DbConstants.BhsConnectionStringName, "photos.AlbumPhotos_GetBySlug", new { slug }, cancellationToken);
        return albums.SingleOrDefault()?.ToDomainModel(photos);
    }

    public async Task<IReadOnlyCollection<Album>> GetAll(CancellationToken cancellationToken = default)
    {
        var results = await E.ExecuteSprocQuery<AlbumDto>(DbConstants.BhsConnectionStringName, "photos.Album_GetAll", cancellationToken: cancellationToken);
        return results.Select(r => r.ToDomainModel()).ToList();
    }
}