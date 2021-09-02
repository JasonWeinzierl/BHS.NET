import { Album } from '@data/schema/album';
import { AlbumPhotos } from '@data/schema/album-photos';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Photo } from '@data/schema/photo';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {
  private baseUrl = '/api/photos';

  constructor(
    private http: HttpClient,
  ) { }

  getAlbums(): Observable<Album[]> {
    return this.http.get<Album[]>(this.baseUrl + '/albums');
  }

  getAlbum(slug: string): Observable<AlbumPhotos> {
    return this.http.get<AlbumPhotos>(this.baseUrl + '/albums/' + slug);
  }

  getPhoto(photoId: number): Observable<Photo> {
    return this.http.get<Photo>(this.baseUrl + '/' + photoId);
  }
}
