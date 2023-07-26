import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Director } from '@data/leadership/models/director';
import { Officer } from '@data/leadership/models/officer';

@Injectable({
  providedIn: 'root',
})
export class LeadershipService {
  private readonly baseUrl = '/api/leadership';

  constructor(
    private readonly http: HttpClient,
  ) { }

  getOfficers(): Observable<Array<Officer>> {
    return this.http.get<Array<Officer>>(this.baseUrl + '/officers');
  }

  getDirectors(): Observable<Array<Director>> {
    return this.http.get<Array<Director>>(this.baseUrl + '/directors');
  }
}
