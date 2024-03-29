import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, of, switchMap, tap } from 'rxjs';
import { BlogService, PostPreview } from '@data/blog';

@Component({
  selector: 'app-posts-search',
  templateUrl: './posts-search.component.html',
  styleUrl: './posts-search.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PostsSearchComponent {
  postsVm$: Observable<{ posts: Array<PostPreview>, error?: string }>;

  private readonly isLoadingSubject = new BehaviorSubject(true);
  public isLoading$: Observable<boolean> = this.isLoadingSubject;

  searchText = '';
  private readonly searchTextSubject = new BehaviorSubject('');

  constructor(
    private readonly blogService: BlogService,
  ) {
    this.postsVm$ = this.searchTextSubject.pipe(
      switchMap(searchText => this.blogService.searchPosts(searchText).pipe(
        map(posts => ({ posts })),
        // Must do the catchError in this inner observable so it doesn't replace the outer observable and break search.
        catchError((err: unknown) => {
          let msg = 'An error occurred.';
          if (err instanceof HttpErrorResponse) {
            msg = err.message;
          } else {
            console.error(err);
          }
          return of({ posts: [], error: msg });
        }),
      )),
      tap(() => { this.isLoadingSubject.next(false); }),
    );
  }

  onSearch(searchText: string): void {
    this.isLoadingSubject.next(true);
    this.searchTextSubject.next(searchText);
  }
}
