import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PostPreview } from '@data/blog';

@Injectable({
  providedIn: 'root',
})
export class AuthorService {
  private readonly baseUrl = '/api/author';

  constructor(
    private readonly http: HttpClient,
  ) { }

  getAuthorPosts(username: string): Observable<Array<PostPreview>> {
    return this.http.get<Array<PostPreview>>(this.baseUrl + '/' + username + '/posts');
  }
}
