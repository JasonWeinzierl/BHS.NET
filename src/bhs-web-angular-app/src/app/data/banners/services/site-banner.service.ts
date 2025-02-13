import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { parseSchema, parseSchemaArray } from '@core/operators/parse-schema.operator';
import { SITE_BANNER_HISTORY_SCHEMA, SiteBanner, SiteBannerHistory, SiteBannerRequest, siteBannerSchema } from '@data/banners';

@Injectable({
  providedIn: 'root',
})
export class SiteBannerService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/banners';

  getEnabled$(): Observable<Array<SiteBanner>> {
    return this.http.get(this.baseUrl + '/current')
      .pipe(parseSchemaArray(siteBannerSchema));
  }

  getHistory$(): Observable<Array<SiteBannerHistory>> {
    return this.http.get(this.baseUrl + '/history')
      .pipe(parseSchemaArray(SITE_BANNER_HISTORY_SCHEMA));
  }

  createBanner$(banner: SiteBannerRequest): Observable<SiteBanner> {
    return this.http.post(this.baseUrl, banner)
      .pipe(parseSchema(siteBannerSchema));
  }

  deleteBanner$(id: string): Observable<void> {
    return this.http.delete<undefined>(`${this.baseUrl}/${id}`);
  }
}
