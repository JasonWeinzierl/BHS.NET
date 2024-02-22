import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryPosts, categoryPostsSchema, CategorySummary, categorySummarySchema, Post, PostPreview, postPreviewSchema, PostRequest, postSchema } from '../models';
import { parseSchema, parseSchemaArray } from '@core/operators/parse-schema.operator';

@Injectable({
  providedIn: 'root',
})
export class BlogService {
  private readonly baseUrl = '/api/blog';

  constructor(
    private readonly http: HttpClient,
  ) { }

  searchPosts(q?: string, from?: Date, to?: Date): Observable<Array<PostPreview>> {
    let params = new HttpParams();
    if (q) {
      params = params.set('q', q);
    }
    if (from) {
      params = params.set('from', from.toISOString());
    }
    if (to) {
      params = params.set('to', to.toISOString());
    }

    return this.http.get(this.baseUrl + '/posts', { params })
      .pipe(parseSchemaArray(postPreviewSchema));
  }

  createPost(request: PostRequest): Observable<Post> {
    return this.http.post(this.baseUrl + '/posts', request) // TODO: investigate withCredentials option. does Auth0 honor it?
      .pipe(parseSchema(postSchema));
  }

  getPost(slug: string): Observable<Post> {
    return this.http.get(this.baseUrl + '/posts/' + slug)
      .pipe(parseSchema(postSchema));
  }

  updatePost(slug: string, request: PostRequest): Observable<Post> {
    return this.http.put(this.baseUrl + '/posts/' + slug, request)
      .pipe(parseSchema(postSchema));
  }

  deletePost(slug: string): Observable<void> {
    return this.http.delete<undefined>(this.baseUrl + '/posts/' + slug);
  }

  getCategories(): Observable<Array<CategorySummary>> {
    return this.http.get(this.baseUrl + '/categories')
      .pipe(parseSchemaArray(categorySummarySchema));
  }

  getCategory(slug: string): Observable<CategoryPosts> {
    return this.http.get(this.baseUrl + '/categories/' + slug)
      .pipe(parseSchema(categoryPostsSchema));
  }
}
