import { AsyncPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouteConfigLoadEnd, RouteConfigLoadStart, Router, RouterOutlet } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { merge, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { InsightsService } from '@core/services/insights.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [RouterOutlet, AsyncPipe],
})
export class AppComponent {
  public loading$: Observable<boolean>;

  public constructor(
    private readonly router: Router,
    private readonly insightsService: InsightsService,
    private readonly auth: AuthService,
  ) {
    this.insightsService.init();
    this.loading$ = merge(
      this.router.events
        .pipe(
          filter(
            event =>
              event instanceof RouteConfigLoadStart ||
              event instanceof RouteConfigLoadEnd,
          ),
          map(event => event instanceof RouteConfigLoadStart),
        ),
      this.auth.isLoading$,
    );
  }
}
