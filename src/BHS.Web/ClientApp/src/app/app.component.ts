import { ActivatedRoute, NavigationEnd, RouteConfigLoadEnd, RouteConfigLoadStart, Router } from '@angular/router';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { filter, map } from 'rxjs/operators';
import { merge, Observable } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';
import { InsightsService } from '@core/services/insights.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default,
})
export class AppComponent implements OnInit {
  public loading$: Observable<boolean>;

  public constructor(
    private readonly router: Router,
    private readonly titleService: Title,
    private readonly activatedRoute: ActivatedRoute,
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

  ngOnInit(): void {
    const appTitle = this.titleService.getTitle();

    this.router.events
      .pipe(
        filter(
          event => event instanceof NavigationEnd,
        ),
        map(() => {
          let child = this.activatedRoute.firstChild;
          while (child?.firstChild) {
            child = child.firstChild;
          }
          const routeTitle: unknown = child?.snapshot.data['title'];
          if (routeTitle && typeof routeTitle === 'string') {
            return routeTitle;
          }
          return appTitle;
        }),
      )
      .subscribe((newTitle: string) => {
        this.setTitle(newTitle);
      });
  }

  private setTitle(newTitle: string): void {
    this.titleService.setTitle(newTitle + ' | Belton Historical Society');
  }
}
