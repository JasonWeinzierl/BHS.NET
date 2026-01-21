import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { SiteBannerService } from '@data/banners';
import parseErrorMessage from '@shared/parseErrorMessage';

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

  readonly errorSignal = signal<string | null>(null);
  readonly deletingBannerIdSignal = signal<string | null>(null);
  readonly successMessageSignal = signal<string | null>(null);

  readonly bannersSignal = toSignal(this.siteBannerService.getHistory$().pipe(
    map(banners => banners.toReversed().map(banner => {
      const sortedChanges = banner.statusChanges
        .toSorted((a, b) => b.dateModified.getTime() - a.dateModified.getTime());
      const pastChanges = sortedChanges
        .filter(c => c.dateModified.getTime() < new Date().getTime());
      return {
        ...banner,
        statusChanges: sortedChanges,
        isEnabled: pastChanges[0]?.isEnabled ?? false,
      };
    })),
    catchError((err: unknown) => {
      const msg = parseErrorMessage(err) ?? 'An unknown error occurred.';
      console.error('Failed to load banners.', err);
      this.errorSignal.set('Failed to load banners. ' + msg);
      return of(null);
    }),
  ), { initialValue: null });

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
        this.deletingBannerIdSignal.set(null);

        // Reload page.
        window.location.reload();
      },
      error: (err: unknown) => {
        const msg = parseErrorMessage(err) ?? 'An unknown error occurred.';
        console.error('Failed to delete banner.', err);
        this.errorSignal.set('Failed to hide banner. ' + msg);
        this.deletingBannerIdSignal.set(null);
      },
    });
  }
}
