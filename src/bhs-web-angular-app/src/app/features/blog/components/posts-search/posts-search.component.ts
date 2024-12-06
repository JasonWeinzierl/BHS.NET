import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { BehaviorSubject, catchError, map, of, switchMap, tap } from 'rxjs';
import { SortByPipe } from '../../../../shared/pipes/sort-by.pipe';
import { PostCardComponent } from '../post-card/post-card.component';
import { BlogService, PostPreview } from '@data/blog';

@Component({
  selector: 'app-posts-search',
  templateUrl: './posts-search.component.html',
  styleUrl: './posts-search.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    FormsModule,
    AlertComponent,
    PostCardComponent,
    SortByPipe,
    AsyncPipe,
  ],
})
export class PostsSearchComponent {
  private readonly blogService = inject(BlogService);

  private readonly searchTextSubject = new BehaviorSubject('');
  postsVm$ = this.searchTextSubject.pipe(
    switchMap(searchText => this.blogService.searchPosts(searchText).pipe(
      map(posts => ({ posts, error: null })),
      // Must do the catchError in this inner observable so it doesn't replace the outer observable and break search.
      catchError((err: unknown) => {
        let msg = 'An error occurred.';
        if (err instanceof HttpErrorResponse) {
          msg = err.message;
        } else {
          console.error(err);
        }
        return of({ posts: [] as Array<PostPreview>, error: msg });
      }),
    )),
    tap(() => { this.isLoading.set(false); }),
  );

  readonly isLoading = signal(true);

  searchText = '';

  onSearch(searchText: string): void {
    this.isLoading.set(true);
    this.searchTextSubject.next(searchText);
  }
}
