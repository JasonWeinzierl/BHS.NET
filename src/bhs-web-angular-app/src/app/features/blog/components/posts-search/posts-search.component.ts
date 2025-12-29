import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BehaviorSubject, catchError, map, of, switchMap, tap } from 'rxjs';
import { PostCardComponent } from '../post-card/post-card.component';
import { BlogService, PostPreview } from '@data/blog';
import parseErrorMessage from '@shared/parseErrorMessage';

@Component({
  selector: 'app-posts-search',
  templateUrl: './posts-search.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    FormsModule,
    PostCardComponent,
    AsyncPipe,
  ],
  host: { 'data-testid': 'PostsSearch' },
})
export class PostsSearchComponent {
  private readonly blogService = inject(BlogService);

  private readonly searchTextSubject$ = new BehaviorSubject('');
  readonly postsVm$ = this.searchTextSubject$.pipe(
    switchMap(searchText => this.blogService.searchPosts(searchText).pipe(
      map(posts => ({
        posts: posts.toSorted((a, b) => b.datePublished.getTime() - a.datePublished.getTime()),
        error: null,
      })),
      // Must do the catchError in this inner observable so it doesn't replace the outer observable and break search.
      catchError((err: unknown) => {
        const msg = parseErrorMessage(err) ?? 'An unknown error occurred.';
        return of({ posts: [] as Array<PostPreview>, error: msg });
      }),
    )),
    tap(() => { this.isLoading.set(false); }),
  );

  readonly isLoading = signal(true);

  searchText = '';

  onSearch(searchText: string): void {
    this.isLoading.set(true);
    this.searchTextSubject$.next(searchText);
  }
}
