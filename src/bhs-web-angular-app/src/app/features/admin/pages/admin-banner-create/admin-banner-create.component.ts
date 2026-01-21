import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { form, FormField, required, validate } from '@angular/forms/signals';
import { Router, RouterLink } from '@angular/router';
import { SiteBannerService } from '@data/banners';
import { AlertTheme } from '@data/banners/models/alert-theme';

@Component({
  selector: 'app-admin-banner-create',
  templateUrl: './admin-banner-create.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    FormField,
  ],
})
export default class AdminBannerCreateComponent {
  private readonly siteBannerService = inject(SiteBannerService);
  private readonly router = inject(Router);

  readonly isSubmittingSignal = signal(false);
  readonly errorSignal = signal<string | null>(null);
  readonly successSignal = signal(false);

  readonly themeOptions = Object.values(AlertTheme);

  readonly bannerModel = signal({
    theme: AlertTheme.Primary as AlertTheme,
    lead: '',
    body: '',
  });

  readonly bannerForm = form(this.bannerModel, schemaPath => {
    required(schemaPath.theme);

    validate(schemaPath, ctx => {
      const lead = ctx.valueOf(schemaPath.lead);
      const body = ctx.valueOf(schemaPath.body);
      if (!lead && !body) {
        return {
          kind: 'atLeastOneRequired',
          message: 'At least one of Lead Text or Body Text must be provided.',
        };
      }
      return null;
    });
  });

  onSubmit(event: SubmitEvent): void {
    event.preventDefault();

    if (this.bannerForm().invalid() || this.isSubmittingSignal()) {
      return;
    }

    this.isSubmittingSignal.set(true);
    this.errorSignal.set(null);
    this.successSignal.set(false);

    const formValue = this.bannerModel();

    // Create the banner request
    this.siteBannerService.createBanner$({
      theme: formValue.theme,
      lead: formValue.lead || undefined,
      body: formValue.body || undefined,
    // eslint-disable-next-line rxjs-angular-x/prefer-async-pipe, rxjs-x/no-ignored-subscription
    }).subscribe({
      next: () => {
        this.successSignal.set(true);
        void setTimeout(() => {
          void this.router.navigate(['/admin/banners']);
        }, 2500);
      },
      error: (err: unknown) => {
        console.error('Failed to create banner.', err);
        this.errorSignal.set('Failed to create banner.');
        this.isSubmittingSignal.set(false);
      },
    });
  }

  onCancel(): void {
    void this.router.navigate(['/admin/banners']);
  }
}
