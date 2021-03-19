import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from '../schema/category';
import { Post } from '../schema/post';
import { PostPreview } from '../schema/post-preview';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  private baseUrl = '/api/blog';

  constructor(private http: HttpClient) { }

  searchPosts(q?: string, from?: Date, to?: Date): Observable<PostPreview[]> {
    const options = { params: new HttpParams() };
    if (q) {
      options.params.set('q', q);
    }
    if (from) {
      options.params.set('from', from.toISOString());
    }
    if (to) {
      options.params.set('to', to.toISOString());
    }

    return this.http.get<PostPreview[]>(this.baseUrl + '/posts', options);
  }

  getPost(slug: string): Observable<Post> {
    return this.http.get<Post>(this.baseUrl + '/posts/' + slug);
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl + '/categories');
  }
}
