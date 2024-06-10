import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { parseSchemaArray } from '@core/operators/parse-schema.operator';
import { Director, directorSchema } from '@data/leadership/models/director';
import { Officer, officerSchema } from '@data/leadership/models/officer';

@Injectable({
  providedIn: 'root',
})
export class LeadershipService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/leadership';

  getOfficers(): Observable<Array<Officer>> {
    return this.http.get(this.baseUrl + '/officers')
      .pipe(parseSchemaArray(officerSchema));
  }

  getDirectors(): Observable<Array<Director>> {
    return this.http.get(this.baseUrl + '/directors')
      .pipe(parseSchemaArray(directorSchema));
  }
}
