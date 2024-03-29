import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContactAlertRequest } from '@data/contact-us';

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
  sendMessage(request: ContactAlertRequest): Observable<void> {
    return this.http.post<undefined>(this.baseUrl, request);
  }
}
