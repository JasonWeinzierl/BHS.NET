import { Author, AuthorService } from '@data/authors';
import { catchError, combineLatest, filter, map, Observable, of, switchMap, tap } from 'rxjs';
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
  vm$: Observable<{ author?: Author, posts: Array<PostPreview> }>;
  isLoading = true;
  error?: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private authorService: AuthorService,
  ) {
    this.vm$ = this.activatedRoute.paramMap
      .pipe(
        map(params => {
          const username = params.get('username');
          if (!username) {
            this.error = 'Failed to get username from URL.';
          }
          return username;
        }),
        filter(username => !!username),
        switchMap(username => combineLatest([
          this.authorService.getAuthor(username!),
          this.authorService.getAuthorPosts(username!)
        ])),
        tap(() => this.isLoading = false),
        map(value => ({ author: value[0], posts: value[1] }) ),
        catchError((error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.error = error.message;
          } else {
            this.error = 'An error occurred.';
          }
          return of();
        }),
      );
  }
}
