import { AsyncPipe, DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { catchError, combineLatest, map, of, startWith } from 'rxjs';
import { LeadershipService } from '@data/leadership';
import parseErrorMessage from '@shared/parseErrorMessage';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    AsyncPipe,
    DatePipe,
  ],
  host: { 'data-testid': 'Organization' },
})
export default class OrganizationComponent {
  private readonly leadershipService = inject(LeadershipService);

  vm$ = combineLatest([this.leadershipService.getOfficers$(), this.leadershipService.getDirectors$()]).pipe(
    map(value => ({ officers: value[0], directors: value[1], isLoading: false, error: null })),
    startWith({ officers: [], directors: [], isLoading: true, error: null }),
    catchError((error: unknown) => {
      const msg = parseErrorMessage(error) ?? 'An unknown error occurred.';
      return of({ officers: [], directors: [], isLoading: false, error: msg });
    }),
  );
}
