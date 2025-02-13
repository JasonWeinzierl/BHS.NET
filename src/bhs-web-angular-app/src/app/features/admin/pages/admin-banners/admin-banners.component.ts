import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { map } from 'rxjs';
import { getBootstrapAlertType, SiteBannerService } from '@data/banners';

@Component({
  selector: 'app-admin-banners',
  templateUrl: './admin-banners.component.html',
  styleUrl: './admin-banners.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
})
export default class AdminBannersComponent {
  private readonly siteBannerService = inject(SiteBannerService);

  readonly bannersSignal = toSignal(this.siteBannerService.getEnabled().pipe(
    map(banners => banners.map(banner => ({
      ...banner,
      bootstrapAlertType: getBootstrapAlertType(banner.theme),
    }))),
  ));
}
