import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ContactAlertRequest } from '@data/contact-us';

@Injectable({
  providedIn: 'root',
})
export class ContactService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/contact-us';

  /**
   * @returns No Content
   */
  sendMessage(request: ContactAlertRequest): Observable<void> {
    return this.http.post<undefined>(this.baseUrl, request);
  }
}
