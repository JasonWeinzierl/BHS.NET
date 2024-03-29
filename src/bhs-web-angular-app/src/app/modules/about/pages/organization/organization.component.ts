import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { catchError, combineLatest, map, Observable, of, startWith } from 'rxjs';
import { Director, LeadershipService, Officer } from '@data/leadership';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrl: './organization.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OrganizationComponent {
  vm$: Observable<{ officers: Array<Officer>, directors: Array<Director>, isLoading: boolean, error?: string }>;

  constructor(
    private readonly leadershipService: LeadershipService,
  ) {
    this.vm$ = combineLatest([this.leadershipService.getOfficers(), this.leadershipService.getDirectors()])
      .pipe(
        map(value => ({ officers: value[0], directors: value[1], isLoading: false }) ),
        startWith({ officers: [], directors: [], isLoading: true }),
        catchError((error: unknown) => {
          let msg = 'An error occurred';
          if (error instanceof HttpErrorResponse) {
            msg = error.message;
          }
          return of({ officers: [], directors: [], isLoading: false, error: msg });
        }),
      );
  }
}
