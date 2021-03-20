import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { ContactAlertRequest } from '@app/data/schema/contact-alert-request';
import { ContactService } from '@app/data/service/contact.service';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent {
  contactForm: FormGroup;
  isSubmitted = false;
  error: string;

  constructor(
    private formBuilder: FormBuilder,
    private contactService: ContactService
  ) {
    this.contactForm = this.formBuilder.group({
      name: '',
      emailAddress: '',
      message: '',
      body: ''
    });
  }

  onSubmit(request: ContactAlertRequest): void {
    this.contactForm.reset();
    request.dateRequested = new Date();
    this.contactService.sendMessage(request).subscribe(() => {
      this.isSubmitted = true;
    }, (error: HttpErrorResponse) => {
      this.error = error.message;
    });
  }
}
