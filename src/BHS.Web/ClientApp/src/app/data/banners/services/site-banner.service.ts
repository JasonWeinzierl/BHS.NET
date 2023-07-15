import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SiteBanner } from '@data/banners/models/site-banner';

@Injectable({
  providedIn: 'root',
})
export class SiteBannerService {
  private readonly baseUrl = '/api/banners';

  constructor(
    private readonly http: HttpClient,
  ) { }

  getEnabled(): Observable<Array<SiteBanner>> {
    return this.http.get<Array<SiteBanner>>(this.baseUrl + '/current');
  }
}
