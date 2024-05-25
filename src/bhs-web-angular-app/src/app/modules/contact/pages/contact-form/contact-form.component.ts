import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AlertModule } from 'ngx-bootstrap/alert';
import { ContactAlertRequest, ContactService } from '@data/contact-us';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrl: './contact-form.component.scss',
  changeDetection: ChangeDetectionStrategy.Default, // TODO: Refactor to OnPush
  standalone: true,
  imports: [
    AlertModule,
    RouterLink,
    FormsModule,
    ReactiveFormsModule,
  ],
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
  errors: Array<string> = [];

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly contactService: ContactService,
  ) { }

  onSubmit(): void {
    this.isSubmitted = true;

    const request: ContactAlertRequest = {
      name: this.contactForm.value.name,
      emailAddress: this.contactForm.value.emailAddress,
      message: this.contactForm.value.message,
      dateRequested: new Date(),
      body: this.contactForm.value.body,
    };

    this.contactService.sendMessage(request)
      .subscribe({
        next: () => {
          this.isAccepted = true;
          this.contactForm.reset();
        },
        error: (err: unknown) => {
          if (err instanceof HttpErrorResponse) {
            // TODO: merge with logic in edit-entry's TODO too.
            const errorBody = err.error as unknown;
            if (typeof errorBody === 'object' && errorBody && 'title' in errorBody && typeof errorBody.title === 'string') {
              this.errors.push(errorBody.title);
            } else {
              this.errors.push(err.message);
            }
          } else {
            this.errors.push('');
          }
          this.isSubmitted = false;
        },
      });
  }

  removeError(index: number): void {
    this.errors.splice(index, 1);
  }
}
