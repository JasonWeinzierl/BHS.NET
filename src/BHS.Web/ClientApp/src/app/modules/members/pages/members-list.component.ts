import { Author, AuthorService } from '@data/authors';
import { catchError, map, Observable, of } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MembersListComponent {
  vm$: Observable<{ authors: Array<Author>, error?: string }>;

  constructor(
    private authorService: AuthorService,
  ) {
    this.vm$ = this.authorService.getAuthors()
      .pipe(
        map(authors => ({ authors }) ),
        catchError((error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            return of({ authors: [], error: error.message });
          } else {
            return of({ authors: [], error: 'An error occurred.' });
          }
        }),
      );
  }

}
