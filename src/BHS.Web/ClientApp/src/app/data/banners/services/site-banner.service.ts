import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SiteBanner } from '@data/banners/models/site-banner';

@Injectable({
  providedIn: 'root'
})
export class SiteBannerService {
  private baseUrl = '/api/banners';

  constructor(
    private http: HttpClient,
  ) { }

  getEnabled(): Observable<SiteBanner[]> {
    return this.http.get<SiteBanner[]>(this.baseUrl + '/current');
  }
}
