import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { AuthorService } from '@data/authors';
import { PostCardComponent } from '@features/blog/components/post-card/post-card.component';
import parseErrorMessage from '@shared/parse-error-message';

@Component({
  selector: 'app-profile-index',
  templateUrl: './profile-index.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    PostCardComponent,
    AsyncPipe,
  ],
})
export class ProfileIndexComponent {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly authorService = inject(AuthorService);

  vm$ = this.activatedRoute.paramMap.pipe(
    map(parameters => {
      const username = parameters.get('username');
      if (!username) {
        throw new Error('Failed to get username from URL.');
      }
      return username;
    }),
    switchMap(username => this.authorService.getAuthorPosts$(username)),
    map(posts => ({ author: posts[0].author, posts, isLoading: false, error: undefined })),
    startWith({ author: undefined, posts: [], isLoading: true, error: undefined }),
    catchError((error: unknown) => {
      const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
      return of({ author: undefined, posts: [], isLoading: false, error: message });
    }),
  );
}
