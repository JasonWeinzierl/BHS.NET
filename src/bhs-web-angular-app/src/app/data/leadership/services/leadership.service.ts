import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { parseSchemaArray } from '@core/operators/parse-schema.operator';
import { Director, directorSchema } from '@data/leadership/models/director';
import { Officer, officerSchema } from '@data/leadership/models/officer';

@Injectable({
  providedIn: 'root',
})
export class LeadershipService {
  private readonly baseUrl = '/api/leadership';

  constructor(
    private readonly http: HttpClient,
  ) { }

  getOfficers(): Observable<Array<Officer>> {
    return this.http.get(this.baseUrl + '/officers')
      .pipe(parseSchemaArray(officerSchema));
  }

  getDirectors(): Observable<Array<Director>> {
    return this.http.get(this.baseUrl + '/directors')
      .pipe(parseSchemaArray(directorSchema));
  }
}
