import { Author, AuthorService } from '@data/authors';
import { catchError, combineLatest, map, Observable, of, startWith, switchMap } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { PostPreview } from '@data/blog';

@Component({
  selector: 'app-profile-index',
  templateUrl: './profile-index.component.html',
  styleUrls: ['./profile-index.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileIndexComponent {
  vm$: Observable<{ author?: Author, posts: Array<PostPreview>, isLoading: boolean, error?: string }>;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authorService: AuthorService,
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
        switchMap(username => combineLatest([
          this.authorService.getAuthor(username),
          this.authorService.getAuthorPosts(username),
        ])),
        map(value => ({ author: value[0], posts: value[1], isLoading: false }) ),
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
