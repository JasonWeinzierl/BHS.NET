import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, Observable, of } from 'rxjs';
import { AlertTheme, alertThemeScheme, SiteBanner, SiteBannerService } from '@data/banners';

interface SiteBannerStyled extends SiteBanner {
  alertType: string,
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent {
  isCollapsed = true;

  banners$: Observable<Array<SiteBannerStyled>>;
  isAuthenticated$ = this.auth.isAuthenticated$;

  constructor(
    private readonly bannerService: SiteBannerService,
    private readonly toastr: ToastrService,
    private readonly auth: AuthService,
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
      ...b,
      alertType: this.getBootstrapAlertType(b.theme),
    }));
  }

  private getBootstrapAlertType(theme: AlertTheme): string {
    if (alertThemeScheme.safeParse(theme).success && theme !== 'None') {
      return theme.toLowerCase();
    } else {
      return 'light';
    }
  }

}
