import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MuseumSchedule, museumScheduleSchema } from './museum-schedule';
import { parseSchema } from '@core/operators/parse-schema.operator';

@Injectable({
  providedIn: 'root',
})
export class MuseumService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/museum';

  getSchedule$(): Observable<MuseumSchedule | null> {
    return this.http.get(this.baseUrl + '/schedule')
      .pipe(parseSchema(museumScheduleSchema));
  }

  updateSchedule$(schedule: MuseumSchedule): Observable<MuseumSchedule> {
    return this.http.put(this.baseUrl + '/schedule', schedule)
      .pipe(parseSchema(museumScheduleSchema));
  }
}
