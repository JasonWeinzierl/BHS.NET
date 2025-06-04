import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { timeout, TimeoutError } from 'rxjs';
import { ContactAlertRequest, ContactService } from '@data/contact-us';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class ContactFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly contactService = inject(ContactService);

  readonly contactForm = this.fb.nonNullable.group({
    name: [''],
    emailAddress: ['', [Validators.required, Validators.email]],
    message: [''],
    body: [''],
  });

  readonly isSubmitted = signal(false);
  readonly isAccepted = signal(false);
  readonly errors = signal<Array<{ id: number; msg: string }>>([]);

  onSubmit(): void {
    this.isSubmitted.set(true);

    const request: ContactAlertRequest = {
      name: this.contactForm.value.name,
      emailAddress: this.contactForm.value.emailAddress,
      message: this.contactForm.value.message,
      dateRequested: new Date(),
      body: this.contactForm.value.body,
    };

    this.contactService.sendMessage(request).pipe(
      timeout(10_000),
    // eslint-disable-next-line rxjs-x/no-ignored-subscription, rxjs-angular-x/prefer-async-pipe
    ).subscribe({
        next: () => {
          this.isAccepted.set(true);
          this.contactForm.reset();
        },
        error: (err: unknown) => {
          if (err instanceof HttpErrorResponse) {
            // TODO: merge with logic in edit-entry's TODO too.
            const errorBody = err.error as unknown;
            if (typeof errorBody === 'object' && errorBody && 'title' in errorBody && typeof errorBody.title === 'string') {
              const errorBodyTitle = errorBody.title;
              this.errors.update(errors => [...errors, { id: errors.length, msg: errorBodyTitle }]);
            } else {
              this.errors.update(errors => [...errors, { id: errors.length, msg: err.message }]);
            }
          } else if (err instanceof TimeoutError) {
            this.errors.update(errors => [...errors, { id: errors.length, msg: 'Something took too long...' }]);
          } else {
            this.errors.update(errors => [...errors, { id: errors.length, msg: '' }]);
          }
          this.isSubmitted.set(false);
        },
      });
  }

  removeError(id: number): void {
    this.errors.update(errors => errors.filter(e => e.id !== id));
  }
}
