import { AsyncPipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, of } from 'rxjs';
import { AlertTheme, alertThemeScheme, SiteBanner, SiteBannerService } from '@data/banners';

interface SiteBannerStyled extends SiteBanner {
  alertType: string;
}

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    AlertModule,
    RouterLink,
    CollapseModule,
    RouterLinkActive,
    BsDropdownModule,
    AsyncPipe,
  ],
})
export class HeaderComponent {
  private readonly bannerService = inject(SiteBannerService);
  private readonly toastr = inject(ToastrService);
  private readonly auth = inject(AuthService);

  isCollapsed = true;

  banners$ = this.bannerService.getEnabled().pipe(
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

  isAuthenticated$ = this.auth.isAuthenticated$;

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
