import { AsyncPipe, JsonPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { jwtDecode } from 'jwt-decode';
import { catchError, map, of } from 'rxjs';

@Component({
  selector: 'app-admin-index',
  templateUrl: './admin-index.component.html',
  styleUrl: './admin-index.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [
    RouterLink,
    AsyncPipe,
    JsonPipe,
  ],
})
export class AdminIndexComponent {
  private readonly auth = inject(AuthService);

  user$ = this.auth.user$;
  accessToken$ = this.auth.getAccessTokenSilently({ cacheMode: 'cache-only' }).pipe(
    map((token: string | undefined) => token ? jwtDecode(token) : { message: 'No token found in cache!' }),
    catchError((err: unknown) => {
      let message = 'An error occurred. ';
      if (typeof err === 'string') {
        message += err;
      } else if (typeof err === 'object' && err && 'message' in err && typeof err.message === 'string') {
        message += err.message;
      }
      console.error(message, err);
      return of({ message });
    }),
  );

  handleLogout(): void {
    this.auth.logout({
      logoutParams: {
        returnTo: window.location.origin,
      },
    });
  }
}
