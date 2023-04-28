import { AlertTheme, SiteBanner, SiteBannerService } from '@data/banners';
import { catchError, map, Observable, of } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

class SiteBannerStyled implements SiteBanner {
 constructor(
   public theme: AlertTheme,
   public alertType: string,
   public lead?: string,
   public body?: string,
 ) { }
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent {
  isCollapsed = true;

  banners$: Observable<Array<SiteBannerStyled>>;
  isAuthenticated$ = this.auth.isAuthenticated$;

  constructor(
    private bannerService: SiteBannerService,
    private toastr: ToastrService,
    private auth: AuthService,
  ) {
    this.banners$ = this.bannerService.getEnabled()
      .pipe(
        map(banners => this.createStyledBanners(banners)),
        catchError((err: unknown) => {
          const title = 'Site banners could not be loaded.';
          let msg = 'An error occurred.';
          if (err instanceof HttpErrorResponse) {
            msg = err.message;
          } else {
            console.error(err);
          }
          this.toastr.error(msg, title);
          return of();
        }),
      );
  }

  private createStyledBanners(banners: Array<SiteBanner>): Array<SiteBannerStyled> {
    return banners.map(b => {
      let alertType = 'light';

      switch (b.theme) {
        case AlertTheme.Primary:
          alertType = 'primary';
          break;
        case AlertTheme.Secondary:
          alertType = 'secondary';
          break;
        case AlertTheme.Success:
          alertType = 'success';
          break;
        case AlertTheme.Danger:
          alertType = 'danger';
          break;
        case AlertTheme.Warning:
          alertType = 'warning';
          break;
        case AlertTheme.Info:
          alertType = 'info';
          break;
      }

      return new SiteBannerStyled(b.theme, alertType, b.lead, b.body);
    });
  }

}
