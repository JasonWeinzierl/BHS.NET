import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryPosts, CategorySummary, Post, PostPreview, PostRequest } from '../models';

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

    return this.http.get<Array<PostPreview>>(this.baseUrl + '/posts', { params });
  }

  createPost(request: PostRequest): Observable<Post> {
    return this.http.post<Post>(this.baseUrl + '/posts', request); // TODO: investigate withCredentials option. does Auth0 honor it?
  }

  getPost(slug: string): Observable<Post> {
    return this.http.get<Post>(this.baseUrl + '/posts/' + slug);
  }

  updatePost(slug: string, request: PostRequest): Observable<Post> {
    return this.http.put<Post>(this.baseUrl + '/posts/' + slug, request);
  }

  deletePost(slug: string): Observable<void> {
    return this.http.delete<undefined>(this.baseUrl + '/posts/' + slug);
  }

  getCategories(): Observable<Array<CategorySummary>> {
    return this.http.get<Array<CategorySummary>>(this.baseUrl + '/categories');
  }

  getCategory(slug: string): Observable<CategoryPosts> {
    return this.http.get<CategoryPosts>(this.baseUrl + '/categories/' + slug);
  }
}
