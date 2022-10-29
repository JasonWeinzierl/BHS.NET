import { ContactAlertRequest, ContactService } from '@data/contact-us';
import { FormBuilder, Validators } from '@angular/forms';
import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent {
  contactForm = this.formBuilder.nonNullable.group({
    name: [''],
    emailAddress: ['', [Validators.required, Validators.email]],
    message: [''],
    body: [''],
  });
  isSubmitted = false;
  isAccepted = false;
  errors: string[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private contactService: ContactService
  ) { }

  onSubmit(): void {
    this.isSubmitted = true;

    const request = new ContactAlertRequest(
      this.contactForm.value.name,
      this.contactForm.value.emailAddress,
      this.contactForm.value.message,
      new Date(),
      this.contactForm.value.body);

    this.contactService.sendMessage(request)
      .subscribe({
        next: () => {
          this.isAccepted = true;
          this.contactForm.reset();
        },
        error: (error: unknown) => {
          if (error instanceof HttpErrorResponse) {
            if (error.error?.title) {
              this.errors.push(error.error.title);
            } else {
              this.errors.push(error.message);
            }
          }
          this.isSubmitted = false;
        }});
  }

  removeError(index: number): void {
    this.errors.splice(index, 1);
  }
}
