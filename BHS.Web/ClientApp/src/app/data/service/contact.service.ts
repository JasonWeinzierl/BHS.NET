import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { ContactAlertRequest } from '@data/schema/contact-alert-request';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  url: string = '/api/contact-us';

  constructor(private http: HttpClient) { }

  sendMessage(request: ContactAlertRequest) {
    return this.http.post(
      this.url,
      JSON.stringify(request),
      {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        responseType: 'text'
      }
    );
  }
}
