import { ActivatedRoute, NavigationEnd, RouteConfigLoadEnd, RouteConfigLoadStart, Router } from '@angular/router';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { filter, map } from 'rxjs/operators';
import { InsightsService } from '@core/services/insights.service';
import { Observable } from 'rxjs';
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
    private router: Router,
    private titleService: Title,
    private activatedRoute: ActivatedRoute,
    private insightsService: InsightsService,
  ) {
    this.insightsService.init();
    this.loading$ = this.router.events
      .pipe(
        filter(
          event =>
            event instanceof RouteConfigLoadStart ||
            event instanceof RouteConfigLoadEnd,
        ),
        map(event => event instanceof RouteConfigLoadStart),
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
          if (child?.snapshot.data['title']) {
            return child.snapshot.data['title'];
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
