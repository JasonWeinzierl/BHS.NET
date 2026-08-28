import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { parseSchemaArray } from '@core/operators/parse-schema.operator';
import { Director, directorSchema } from '@data/leadership/models/director';
import { DirectorRequest } from '@data/leadership/models/director-request';
import { Office, officeSchema } from '@data/leadership/models/office';
import { Officer, officerSchema } from '@data/leadership/models/officer';
import { OfficerRequest } from '@data/leadership/models/officer-request';

@Injectable({
  providedIn: 'root',
})
export class LeadershipService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/leadership';

  getOfficers$(): Observable<Array<Officer>> {
    return this.http.get(this.baseUrl + '/officers')
      .pipe(parseSchemaArray(officerSchema));
  }

  getOffices$(): Observable<Array<Office>> {
    return this.http.get(this.baseUrl + '/offices')
      .pipe(parseSchemaArray(officeSchema));
  }

  getDirectors$(): Observable<Array<Director>> {
    return this.http.get(this.baseUrl + '/directors')
      .pipe(parseSchemaArray(directorSchema));
  }

  updateOfficers$(officers: Array<OfficerRequest>): Observable<Array<Officer>> {
    return this.http.put(this.baseUrl + '/officers', officers)
      .pipe(parseSchemaArray(officerSchema));
  }

  addDirectors$(directors: Array<DirectorRequest>): Observable<Array<Director>> {
    return this.http.post(this.baseUrl + '/directors', directors)
      .pipe(parseSchemaArray(directorSchema));
  }

  deleteDirector$(director: DirectorRequest): Observable<unknown> {
    const yearString = String(director.year);
    const encodedName = encodeURIComponent(director.name);
    return this.http.delete(`${this.baseUrl}/directors/${yearString}/${encodedName}`);
  }
}
