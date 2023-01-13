import { catchError, Observable, of } from 'rxjs';
import { Director, LeadershipService, Officer } from '@data/leadership';
import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent {
  officers$: Observable<Officer[]>;
  directors$: Observable<Director[]>;

  errors: string[] = [];

  constructor(
    private leadershipService: LeadershipService
  ) {
    this.officers$ = this.leadershipService.getOfficers().pipe(catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse) {
        this.errors.push(error.message);
      }
      return of([]);
    }));
    this.directors$ = this.leadershipService.getDirectors().pipe(catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse) {
        this.errors.push(error.message);
      }
      return of([]);
    }));
  }
}
