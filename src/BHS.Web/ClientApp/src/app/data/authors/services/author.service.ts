import { Author } from '@data/authors/models/author';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PostPreview } from '@data/blog';

@Injectable({
  providedIn: 'root',
})
export class AuthorService {
  private baseUrl = '/api/author';

  constructor(
    private http: HttpClient,
  ) { }

  getAuthors(): Observable<Author[]> {
    return this.http.get<Author[]>(this.baseUrl);
  }

  getAuthor(username: string): Observable<Author> {
    return this.http.get<Author>(this.baseUrl + '/' + username);
  }

  getAuthorPosts(username: string): Observable<PostPreview[]> {
    return this.http.get<PostPreview[]>(this.baseUrl + '/' + username + '/posts');
  }
}
