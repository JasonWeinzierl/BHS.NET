import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { parseSchemaArray } from '@core/operators/parse-schema.operator';
import { SiteBanner, siteBannerSchema } from '@data/banners/models/site-banner';

@Injectable({
  providedIn: 'root',
})
export class SiteBannerService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/banners';

  getEnabled(): Observable<Array<SiteBanner>> {
    return this.http.get(this.baseUrl + '/current')
      .pipe(parseSchemaArray(siteBannerSchema));
  }
}
