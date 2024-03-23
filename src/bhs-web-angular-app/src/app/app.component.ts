import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, RouteConfigLoadEnd, RouteConfigLoadStart, Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { merge, Observable, Subscription } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { InsightsService } from '@core/services/insights.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.Default,
})
export class AppComponent implements OnInit, OnDestroy {
  public loading$: Observable<boolean>;
  private titleSub?: Subscription;

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

    this.titleSub = this.router.events
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
      .subscribe({
        next: (newTitle: string) => {
          this.setTitle(newTitle);
        },
        error: (err: unknown) => {
          console.error(err);
        },
      });
  }

  ngOnDestroy(): void {
    this.titleSub?.unsubscribe();
  }

  private setTitle(newTitle: string): void {
    this.titleService.setTitle(newTitle + ' | Belton Historical Society');
  }
}
