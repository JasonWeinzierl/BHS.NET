import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { catchError, map, of, startWith, switchMap } from 'rxjs';
import { PostCardComponent } from '../../blog/components/post-card/post-card.component';
import { AuthorService } from '@data/authors';

@Component({
  selector: 'app-profile-index',
  templateUrl: './profile-index.component.html',
  styleUrl: './profile-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    AlertComponent,
    PostCardComponent,
    AsyncPipe,
  ],
})
export class ProfileIndexComponent {
  private readonly activatedRoute = inject(ActivatedRoute);
  private readonly authorService = inject(AuthorService);

  vm$ = this.activatedRoute.paramMap.pipe(
    map(params => {
      const username = params.get('username');
      if (!username) {
        throw new Error('Failed to get username from URL.');
      }
      return username;
    }),
    switchMap(username => this.authorService.getAuthorPosts(username)),
    map(posts => ({ author: posts[0].author, posts, isLoading: false, error: null })),
    startWith({ author: null, posts: [], isLoading: true, error: null }),
    catchError((error: unknown) => {
      let msg = 'An error occurred.';
      if (error instanceof HttpErrorResponse) {
        msg = error.message;
      } else {
        console.error(error);
      }
      return of({ author: null, posts: [], isLoading: false, error: msg });
    }),
  );
}
