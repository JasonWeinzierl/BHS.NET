import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { ContactAlertRequest } from '@data/schema/contact-alert-request';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private baseUrl = '/api/contact-us';

  constructor(private http: HttpClient) { }

  sendMessage(request: ContactAlertRequest) {
    return this.http.post<ContactAlertRequest>(this.baseUrl, request);
  }
}
