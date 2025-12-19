import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouteConfigLoadEnd, RouteConfigLoadStart, Router, RouterOutlet } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { filter, map, merge } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterOutlet,
    AsyncPipe,
  ],
})
export class AppComponent {
  private readonly router = inject(Router);
  private readonly auth = inject(AuthService);

  protected readonly loading$ = merge(
    this.router.events
      .pipe(
        filter(
          event =>
            event instanceof RouteConfigLoadStart
            || event instanceof RouteConfigLoadEnd,
        ),
        map(event => event instanceof RouteConfigLoadStart),
      ),
    this.auth.isLoading$,
  );
}
