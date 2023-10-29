import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { parseSchema, parseSchemaArray } from '@core/operators/parse-schema.operator';
import { Album, albumSchema } from '@data/photos/models/album';
import { AlbumPhotos, albumPhotosSchema } from '@data/photos/models/album-photos';
import { Photo, photoSchema } from '@data/photos/models/photo';

@Injectable({
  providedIn: 'root',
})
export class PhotosService {
  private readonly baseUrl = '/api/photos';

  constructor(
    private readonly http: HttpClient,
  ) { }

  getAlbums(): Observable<Array<Album>> {
    return this.http.get(this.baseUrl + '/albums')
      .pipe(parseSchemaArray(albumSchema));
  }

  getAlbum(slug: string): Observable<AlbumPhotos> {
    return this.http.get(this.baseUrl + '/albums/' + slug)
      .pipe(parseSchema(albumPhotosSchema));
  }

  getPhoto(albumSlug: string, photoId: string): Observable<Photo> {
    return this.http.get(this.baseUrl + '/albums/' + albumSlug + '/photos/' + photoId)
      .pipe(parseSchema(photoSchema));
  }
}
