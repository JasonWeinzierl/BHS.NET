import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, Observable, of, startWith, switchMap } from 'rxjs';
import { Author, AuthorService } from '@data/authors';
import { PostPreview } from '@data/blog';

@Component({
  selector: 'app-profile-index',
  templateUrl: './profile-index.component.html',
  styleUrl: './profile-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileIndexComponent {
  vm$: Observable<{ author?: Author | null, posts: Array<PostPreview>, isLoading: boolean, error?: string }>;

  constructor(
    private readonly activatedRoute: ActivatedRoute,
    private readonly authorService: AuthorService,
  ) {
    this.vm$ = this.activatedRoute.paramMap
      .pipe(
        map(params => {
          const username = params.get('username');
          if (!username) {
            throw new Error('Failed to get username from URL.');
          }
          return username;
        }),
        switchMap(username => this.authorService.getAuthorPosts(username)),
        map(posts => ({ author: posts[0].author, posts, isLoading: false }) ),
        startWith({ posts: [], isLoading: true }),
        catchError((error: unknown) => {
          let msg = 'An error occurred.';
          if (error instanceof HttpErrorResponse) {
            msg = error.message;
          } else {
            console.error(error);
          }
          return of({ posts: [], isLoading: false, error: msg });
        }),
      );
  }
}
