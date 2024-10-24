import { AsyncPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { catchError, combineLatest, map, of, startWith } from 'rxjs';
import { LeadershipService } from '@data/leadership';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrl: './organization.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AlertComponent,
    AsyncPipe,
    DatePipe,
  ],
})
export default class OrganizationComponent {
  private readonly leadershipService = inject(LeadershipService);

  vm$ = combineLatest([this.leadershipService.getOfficers(), this.leadershipService.getDirectors()]).pipe(
    map(value => ({ officers: value[0], directors: value[1], isLoading: false, error: null })),
    startWith({ officers: [], directors: [], isLoading: true, error: null }),
    catchError((error: unknown) => {
      let msg = 'An error occurred';
      if (error instanceof HttpErrorResponse) {
        msg = error.message;
      }
      return of({ officers: [], directors: [], isLoading: false, error: msg });
    }),
  );
}
