import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { parseSchemaArray } from '@core/operators/parse-schema.operator';
import { PostPreview, postPreviewSchema } from '@data/blog';
import { Author, authorSchema } from '../models/author';

@Injectable({
  providedIn: 'root',
})
export class AuthorService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/authors';

  getAuthors$(authUserId: string): Observable<Array<Author>> {
    return this.http.get(this.baseUrl + '?authUserId=' + authUserId)
      .pipe(parseSchemaArray(authorSchema));
  }

  getAuthorPosts$(username: string): Observable<Array<PostPreview>> {
    return this.http.get(this.baseUrl + '/' + username + '/posts')
      .pipe(parseSchemaArray(postPreviewSchema));
  }
}
