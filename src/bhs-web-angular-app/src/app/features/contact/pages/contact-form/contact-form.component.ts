import { ChangeDetectionStrategy, Component, ElementRef, inject, signal, viewChild } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { timeout, TimeoutError } from 'rxjs';
import { InsightsService } from '@core/services/insights.service';
import { ContactAlertRequest, ContactService } from '@data/contact-us';
import parseErrorMessage from '@shared/parseErrorMessage';

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
  private readonly insights = inject(InsightsService);

  readonly contactFormEl = viewChild.required<ElementRef<HTMLElement>>('contactUsHeader');

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
    this.insights.submitContactForm();

    this.isSubmitted.set(true);
    this.contactFormEl().nativeElement.scrollIntoView({
      behavior: 'smooth',
      block: 'start',
      inline: 'nearest',
    });

    const formValue = this.contactForm.getRawValue();
    const request: ContactAlertRequest = {
      name: formValue.name,
      emailAddress: formValue.emailAddress,
      message: formValue.message,
      dateRequested: new Date(),
      body: formValue.body,
    };

    this.contactService.sendMessage$(request).pipe(
      timeout(10_000),
    // eslint-disable-next-line rxjs-x/no-ignored-subscription, rxjs-angular-x/prefer-async-pipe
    ).subscribe({
        next: () => {
          this.isAccepted.set(true);
          this.contactForm.reset();
        },
        error: (err: unknown) => {
          if (err instanceof TimeoutError) {
            this.errors.update(errors => [...errors, { id: errors.length, msg: 'Something took too long...' }]);
          } else {
            const msg = parseErrorMessage(err) ?? 'An unexpected error occurred.';
            this.errors.update(errors => [...errors, { id: errors.length, msg }]);
          }
          this.isSubmitted.set(false);
        },
      });
  }

  removeError(id: number): void {
    this.errors.update(errors => errors.filter(e => e.id !== id));
  }
}
