import { HttpClient, HttpParams } from '@angular/common/http';
import { CategoryPosts } from '@data/blog/models/category-posts';
import { CategorySummary } from '@data/blog/models/category-summary';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from '@data/blog/models/post';
import { PostPreview } from '@data/blog/models/post-preview';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  private baseUrl = '/api/blog';

  constructor(
    private http: HttpClient,
  ) { }

  searchPosts(q?: string, from?: Date, to?: Date): Observable<PostPreview[]> {
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

    return this.http.get<PostPreview[]>(this.baseUrl + '/posts', { params });
  }

  getPost(slug: string): Observable<Post> {
    return this.http.get<Post>(this.baseUrl + '/posts/' + slug);
  }

  getCategories(): Observable<CategorySummary[]> {
    return this.http.get<CategorySummary[]>(this.baseUrl + '/categories');
  }

  getCategory(slug: string): Observable<CategoryPosts> {
    return this.http.get<CategoryPosts>(this.baseUrl + '/categories/' + slug);
  }
}
