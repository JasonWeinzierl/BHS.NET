import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { ContactAlertRequest, ContactService } from '@data/contact-us';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrl: './contact-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    AlertComponent,
    RouterLink,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class ContactFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly contactService = inject(ContactService);

  contactForm = this.fb.nonNullable.group({
    name: [''],
    emailAddress: ['', [Validators.required, Validators.email]],
    message: [''],
    body: [''],
  });

  isSubmitted = signal(false);
  isAccepted = signal(false);
  errors = signal<Array<{ id: number; msg: string }>>([]);

  onSubmit(): void {
    this.isSubmitted.set(true);

    const request: ContactAlertRequest = {
      name: this.contactForm.value.name,
      emailAddress: this.contactForm.value.emailAddress,
      message: this.contactForm.value.message,
      dateRequested: new Date(),
      body: this.contactForm.value.body,
    };

    this.contactService.sendMessage(request)
      .pipe(takeUntilDestroyed())
      // eslint-disable-next-line rxjs-x/no-ignored-subscription
      .subscribe({
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
