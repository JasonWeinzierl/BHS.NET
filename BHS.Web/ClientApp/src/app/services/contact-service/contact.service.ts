import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ContactModel } from 'src/app/models/contact.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  url: string = '/api/contact-us';

  constructor(private http: HttpClient) { }

  sendMessage(contactData: ContactModel) {
    return this.http.post(this.url,
      JSON.stringify(contactData),
      { headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
        responseType: 'text'});
  }
}
