import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Director } from '../schema/director';
import { Officer } from '../schema/officer';

@Injectable({
  providedIn: 'root'
})
export class LeadershipService {
  private baseUrl = '/api/leadership';

  constructor(private http: HttpClient) { }

  getOfficers() {
    return this.http.get<Officer[]>(this.baseUrl + '/officers');
  }

  getDirectors() {
    return this.http.get<Director[]>(this.baseUrl + '/directors');
  }
}
