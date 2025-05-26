import { AsyncPipe, DatePipe, NgOptimizedImage } from '@angular/common';
import { ChangeDetectionStrategy, Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { jwtDecode } from 'jwt-decode';
import { catchError, map, of, startWith } from 'rxjs';

@Component({
  selector: 'app-admin-index',
  templateUrl: './admin-index.component.html',
  styleUrl: './admin-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    AsyncPipe,
    NgOptimizedImage,
    DatePipe,
  ],
})
export class AdminIndexComponent {
  private readonly auth = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);

  readonly user$ = this.auth.user$;
  readonly permissionsVm$ = this.auth.getAccessTokenSilently({ cacheMode: 'cache-only' }).pipe(
    map((token: string | undefined) => {
      if (!token) {
        return {
          type: 'error' as const,
          message: 'No token found in cache!',
        };
      }
      const jwt = jwtDecode(token);
      return {
        type: 'success' as const,
        permissions: 'permissions' in jwt && Array.isArray(jwt.permissions) && jwt.permissions.every(p => typeof p === 'string') ? jwt.permissions : [],
      };
    }),
    catchError((err: unknown) => {
      let message = 'An error occurred. ';
      if (typeof err === 'string') {
        message += err;
      } else if (typeof err === 'object' && err && 'message' in err && typeof err.message === 'string') {
        message += err.message;
      }
      console.error(message, err);
      return of({ type: 'error' as const, message });
    }),
    startWith({ type: 'loading' as const, message: 'Loading...' }),
  );

  handleLogout(): void {
    this.auth.logout({
      logoutParams: {
        returnTo: window.location.origin,
      },
    })
      .pipe(takeUntilDestroyed(this.destroyRef))
      // eslint-disable-next-line rxjs-angular-x/prefer-async-pipe
      .subscribe({
        error: (err: unknown) => { console.error('An error occurred while logging out:', err); },
      });
  }
}
