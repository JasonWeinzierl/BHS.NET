import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SITE_BANNER_HISTORY_SCHEMA, SiteBannerHistory } from '../models/site-banner-history';
import { parseSchemaArray } from '@core/operators/parse-schema.operator';
import { SiteBanner, siteBannerSchema } from '@data/banners/models/site-banner';

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
}
