import { Author, AuthorService } from '@data/authors';
import { catchError, map, Observable, of, startWith } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MembersListComponent {
  vm$: Observable<{ authors: Array<Author>, isLoading: boolean, error?: string }>;

  constructor(
    private authorService: AuthorService,
  ) {
    this.vm$ = this.authorService.getAuthors()
      .pipe(
        map(authors => ({ authors, isLoading: false }) ),
        startWith({ authors: [], isLoading: true }),
        catchError((error: unknown) => {
          let msg = 'An error occurred.';
          if (error instanceof HttpErrorResponse) {
            msg = error.message;
          } else {
            console.error(error);
          }
          return of({ authors: [], isLoading: false, error: msg });
        }),
      );
  }

}
