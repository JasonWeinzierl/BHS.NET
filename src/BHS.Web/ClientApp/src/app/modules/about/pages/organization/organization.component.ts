import { Component, OnInit } from '@angular/core';
import { Director, LeadershipService, Officer } from '@data/leadership';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent implements OnInit {
  officers?: Officer[];
  directors?: Director[];

  errors: string[] = [];

  constructor(
    private leadershipService: LeadershipService
  ) { }

  ngOnInit(): void {
    this.leadershipService.getOfficers()
      .subscribe(
        response => this.officers = response,
        (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.errors.push(error.message);
          }
        }
      );

    this.leadershipService.getDirectors()
      .subscribe(
        response => this.directors = response,
        (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            this.errors.push(error.message);
          }
        }
      );
  }
}
