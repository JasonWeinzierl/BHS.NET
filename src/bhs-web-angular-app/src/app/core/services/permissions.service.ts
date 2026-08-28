import { inject, Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { jwtDecode } from 'jwt-decode';
import { catchError, map, Observable, of, switchMap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PermissionsService {
  private readonly auth = inject(AuthService);

  readonly isAuthenticated$ = this.auth.isAuthenticated$;

  hasPermission$(permission: string): Observable<boolean> {
    return this.auth.isAuthenticated$.pipe(
      switchMap(isAuthenticated => isAuthenticated
        ? this.auth.getAccessTokenSilently({ cacheMode: 'cache-only' })
        : of(undefined)),
      map(token => {
        if (!token) {
          return false;
        }
        const jwt = jwtDecode(token);
        return 'permissions' in jwt
          && Array.isArray(jwt.permissions)
          && jwt.permissions.includes(permission);
      }),
      catchError(() => of(false)),
    );
  }
}
