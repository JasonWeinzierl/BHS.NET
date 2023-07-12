import { ALERT_THEMES, AlertTheme, SiteBanner, SiteBannerService } from '@data/banners';
import { catchError, map, Observable, of } from 'rxjs';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

interface SiteBannerStyled {
  alertType: string,
  lead: string | null,
  body: string | null,
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
    return banners.map(b => ({
      lead: b.lead,
      body: b.body,
      alertType: this.getBootstrapAlertType(b.theme),
    }));
  }

  private getBootstrapAlertType(theme: AlertTheme): string {
    if (ALERT_THEMES.includes(theme) && theme !== 'None') {
      return theme.toLowerCase();
    } else {
      return 'light';
    }
  }

}
