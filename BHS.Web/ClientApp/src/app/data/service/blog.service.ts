import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '../schema/post';
import { PostPreview } from '../schema/post-preview';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  private baseUrl = '/api/blog';

  constructor(private http: HttpClient) { }

  searchPosts(q?: string, from?: Date, to?: Date) {
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

  getPost(slug: string) {
    return this.http.get<Post>(this.baseUrl + '/posts/' + slug);
  }
}
