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
}
