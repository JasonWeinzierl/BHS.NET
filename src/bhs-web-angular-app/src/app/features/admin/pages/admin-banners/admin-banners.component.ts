import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { catchError, map, of } from 'rxjs';
import { getBootstrapAlertType, SiteBannerService } from '@data/banners';

@Component({
  selector: 'app-admin-banners',
  templateUrl: './admin-banners.component.html',
  styleUrl: './admin-banners.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    DatePipe,
  ],
})
export default class AdminBannersComponent {
  private readonly siteBannerService = inject(SiteBannerService);

  readonly errorSignal = signal<string | null>(null);
  readonly bannersSignal = toSignal(this.siteBannerService.getHistory$().pipe(
    map(banners => banners.map(banner => ({
      ...banner,
      bootstrapAlertType: getBootstrapAlertType(banner.theme),
      isEnabled: banner.statusChanges
        .filter(c => c.dateModified.getTime() <= new Date().getTime())
        .toSorted((a, b) => b.dateModified.getTime() - a.dateModified.getTime())
        .at(0)?.isEnabled ?? false,
    }))),
    catchError((err: unknown) => {
      console.error('Failed to load banners.', err);
      this.errorSignal.set('Failed to load banners.');
      return of(null);
    }),
  ), { initialValue: null });
}
