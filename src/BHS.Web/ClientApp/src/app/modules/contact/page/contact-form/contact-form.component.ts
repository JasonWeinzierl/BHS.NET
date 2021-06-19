import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { ContactAlertRequest } from '@data/schema/contact-alert-request';
import { ContactService } from '@data/service/contact.service';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent {
  contactForm: FormGroup;
  isSubmitted = false;
  isAccepted = false;
  errors: string[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private contactService: ContactService
  ) {
    this.contactForm = this.formBuilder.group({
      name: [''],
      emailAddress: ['', [Validators.required, Validators.email]],
      message: [''],
      body: [''],
    });
  }

  onSubmit(request: ContactAlertRequest): void {
    this.isSubmitted = true;
    request.dateRequested = new Date();
    this.contactService.sendMessage(request)
      .subscribe(() => {
        this.isAccepted = true;
        this.contactForm.reset();
      },
      (error: HttpErrorResponse) => {
        if (error.error?.title) {
          this.errors.push(error.error.title);
        } else {
          this.errors.push(error.message);
        }
        this.isSubmitted = false;
      });
  }

  removeError(index: number): void {
    this.errors.splice(index, 1);
  }
}
