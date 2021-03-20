import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Album } from '../schema/album';
import { Photo } from '../schema/photo';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {
  private baseUrl = '/api/photos';

  constructor(private http: HttpClient) { }

  getAlbums(): Observable<Album[]> {
    return this.http.get<Album[]>(this.baseUrl + '/albums');
  }

  // todo: change to exhibit or albumphotos model to include array of photos

  getAlbum(slug: string): Observable<Album> {
    return this.http.get<Album>(this.baseUrl + '/albums/' + slug);
  }

  getPhotos(albumSlug: string): Observable<Photo[]> {
    return this.http.get<Photo[]>(this.baseUrl + '/albums/' + albumSlug + '/photos');
  }

  getPhoto(albumSlug: string, photoId: number): Observable<Photo> {
    return this.http.get<Photo>(this.baseUrl + '/albums/' + albumSlug + '/photos/' + photoId);
  }
}
