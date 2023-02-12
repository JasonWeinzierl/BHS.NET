import { catchError, combineLatest, map, Observable, of } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Director, LeadershipService, Officer } from '@data/leadership';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OrganizationComponent {
  vm$: Observable<{ officers: Array<Officer>, directors: Array<Director>, error?: string }>;

  constructor(
    private leadershipService: LeadershipService
  ) {
    this.vm$ = combineLatest([this.leadershipService.getOfficers(), this.leadershipService.getDirectors()])
      .pipe(
        map(value => ({ officers: value[0], directors: value[1] }) ),
        catchError((error: unknown) => {
          let msg = 'An error occurred';
          if (error instanceof HttpErrorResponse) {
            msg = error.message;
          }
          return of({ officers: [], directors: [], error: msg });
        }),
      );
  }
}
