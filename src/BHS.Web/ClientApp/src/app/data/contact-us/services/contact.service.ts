import { ContactAlertRequest } from '@data/contact-us';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ContactService {
  private readonly baseUrl = '/api/contact-us';

  constructor(
    private readonly http: HttpClient,
  ) { }

  /**
   * @returns No Content
   */
  sendMessage(request: ContactAlertRequest): Observable<object> {
    return this.http.post(this.baseUrl, request);
  }
}
