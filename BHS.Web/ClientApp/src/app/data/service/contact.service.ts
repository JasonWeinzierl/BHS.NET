import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { ContactAlertRequest } from '@data/schema/contact-alert-request';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  url = '/api/contact-us';

  constructor(private http: HttpClient) { }

  sendMessage(request: ContactAlertRequest) {
    return this.http.post(
      this.url,
      JSON.stringify(request),
      {
        headers: new HttpHeaders()
          .set('content-type', 'application/json'),
        responseType: 'text'
      }
    );
  }
}
