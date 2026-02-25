import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { SiteBannerService } from '@data/banners';
import parseErrorMessage from '@shared/parse-error-message';

@Component({
  selector: 'app-admin-banners',
  templateUrl: './admin-banners.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    DatePipe,
    RouterLink,
  ],
})
export default class AdminBannersComponent {
  private readonly siteBannerService = inject(SiteBannerService);

  readonly errorSignal = signal<string | undefined>(undefined);
  readonly deletingBannerIdSignal = signal<string | undefined>(undefined);
  readonly successMessageSignal = signal<string | undefined>(undefined);

  readonly bannersSignal = toSignal(this.siteBannerService.getHistory$().pipe(
    map(banners => banners.toReversed().map(banner => {
      const sortedChanges = banner.statusChanges
        .toSorted((a, b) => b.dateModified.getTime() - a.dateModified.getTime());
      const pastChange = sortedChanges
        .find(c => c.dateModified.getTime() < Date.now());
      return {
        ...banner,
        statusChanges: sortedChanges,
        isEnabled: pastChange?.isEnabled ?? false,
      };
    })),
    catchError((error: unknown) => {
      const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
      console.error('Failed to load banners.', error);
      this.errorSignal.set('Failed to load banners. ' + message);
      return of(undefined);
    }),
  ), { initialValue: undefined });

  onDeleteBanner(bannerId: string, event: Event): void {
    event.preventDefault();

    if (this.deletingBannerIdSignal()) {
      return;
    }

    if (!confirm('Are you sure you want to hide this banner? This will disable it immediately but preserve its history.')) {
      return;
    }

    this.deletingBannerIdSignal.set(bannerId);

    // eslint-disable-next-line rxjs-angular-x/prefer-async-pipe, rxjs-x/no-ignored-subscription
    this.siteBannerService.deleteBanner$(bannerId).subscribe({
      next: () => {
        this.successMessageSignal.set('Banner hidden successfully.');
        this.deletingBannerIdSignal.set(undefined);

        // Reload page.
        globalThis.location.reload();
      },
      error: (error: unknown) => {
        const message = parseErrorMessage(error) ?? 'An unknown error occurred.';
        console.error('Failed to delete banner.', error);
        this.errorSignal.set('Failed to hide banner. ' + message);
        this.deletingBannerIdSignal.set(undefined);
      },
    });
  }
}
