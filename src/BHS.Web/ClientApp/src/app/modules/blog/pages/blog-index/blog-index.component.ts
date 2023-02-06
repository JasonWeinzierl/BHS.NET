import { BehaviorSubject, Observable } from 'rxjs';
import { BlogService, CategorySummary, PostPreview } from '@data/blog';
import { Component, TrackByFunction } from '@angular/core';
import { finalize, switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'app-blog-index',
  templateUrl: './blog-index.component.html',
  styleUrls: ['./blog-index.component.scss']
})
export class BlogIndexComponent {
  posts$: Observable<PostPreview[]>;
  categories$: Observable<CategorySummary[]>;
  private searchTextSubject = new BehaviorSubject('');

  searchText = '';
  loadingCategories = true;
  loadingPosts = true;

  constructor(
    private blogService: BlogService,
  ) {
    this.posts$ = this.searchTextSubject.asObservable()
      .pipe(
        switchMap((searchText) => this.blogService.searchPosts(searchText)),
        tap(_ => this.loadingPosts = false),
      );
    this.categories$ = this.blogService.getCategories()
      .pipe(
        finalize(() => this.loadingCategories = false),
      );
  }

  onSearch(searchText: string): void {
    this.loadingPosts = true;
    this.searchTextSubject.next(searchText);
  }

  trackPostPreview: TrackByFunction<PostPreview> = (_, item) => {
    return item.slug;
  };
}
