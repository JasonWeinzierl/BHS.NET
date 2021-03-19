import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Director } from '@data/schema/director';
import { Officer } from '@data/schema/officer';
import { LeadershipService } from '@data/service/leadership.service';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent implements OnInit {
  officers: Officer[];
  directors: Director[];

  errors: string[] = [];

  constructor(
    private leadershipService: LeadershipService
  ) { }

  ngOnInit(): void {
    this.leadershipService.getOfficers()
      .subscribe(response => this.officers = response,
        (error: HttpErrorResponse) => this.errors.push(error.message));
    this.leadershipService.getDirectors()
      .subscribe(response => this.directors = response,
        (error: HttpErrorResponse) => this.errors.push(error.message));
  }
}
