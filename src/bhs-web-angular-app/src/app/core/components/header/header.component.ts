import { AsyncPipe, NgClass } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { ToastrService } from 'ngx-toastr';
import { catchError, of, scan, startWith, Subject, switchMap } from 'rxjs';
import { SiteBannerService } from '@data/banners';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    RouterLinkActive,
    AsyncPipe,
    NgClass,
  ],
})
export class HeaderComponent {
  private readonly bannerService = inject(SiteBannerService);
  private readonly toastr = inject(ToastrService);
  private readonly auth = inject(AuthService);

  readonly isCollapsed = signal(true);
  readonly isContentMenuCollapsed = signal(true);
  readonly isAboutMenuCollapsed = signal(true);

  private readonly hideSubject$ = new Subject<string>();
  readonly banners$ = this.bannerService.getEnabled$().pipe(
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
    switchMap(banners => this.hideSubject$.pipe(
      scan((acc, bannerId) => acc.filter(banner => banner.id !== bannerId), banners),
      startWith(banners),
    )),
  );

  readonly isAuthenticated$ = this.auth.isAuthenticated$;

  hideBanner(bannerId: string): void {
    this.hideSubject$.next(bannerId);
  }

  toggleNavbar(): void {
    this.isCollapsed.update(collapsed => !collapsed);
    this.isContentMenuCollapsed.set(true);
    this.isAboutMenuCollapsed.set(true);
  }

  toggleContentMenu(force?: boolean): void {
    this.isContentMenuCollapsed.update(collapsed => force ?? !collapsed);
  }

  toggleAboutMenu(force?: boolean): void {
    this.isAboutMenuCollapsed.update(collapsed => force ?? !collapsed);
  }
}
