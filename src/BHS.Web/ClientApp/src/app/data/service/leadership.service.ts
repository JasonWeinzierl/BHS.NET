import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Director } from '@data/schema/director';
import { Officer } from '@data/schema/officer';

@Injectable({
  providedIn: 'root'
})
export class LeadershipService {
  private baseUrl = '/api/leadership';

  constructor(
    private http: HttpClient,
  ) { }

  getOfficers(): Observable<Officer[]> {
    return this.http.get<Officer[]>(this.baseUrl + '/officers');
  }

  getDirectors(): Observable<Director[]> {
    return this.http.get<Director[]>(this.baseUrl + '/directors');
  }
}
