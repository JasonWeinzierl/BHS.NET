import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouteConfigLoadEnd, RouteConfigLoadStart, Router, RouterOutlet } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { merge } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { InsightsService } from '@core/services/insights.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [RouterOutlet, AsyncPipe],
})
export class AppComponent {
  private readonly router = inject(Router);
  private readonly insightsService = inject(InsightsService);
  private readonly auth = inject(AuthService);

  public loading$ = merge(
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

  constructor() {
    this.insightsService.init();
  }
}
