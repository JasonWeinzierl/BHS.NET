import { ContactAlertRequest } from '@data/schema/contact-alert-request';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private baseUrl = '/api/contact-us';

  constructor(
    private http: HttpClient,
  ) { }

  /**
   * @returns No Content
   */
  sendMessage(request: ContactAlertRequest): Observable<any> {
    return this.http.post(this.baseUrl, request);
  }
}
