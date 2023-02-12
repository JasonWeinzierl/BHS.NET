import { ActivatedRoute, NavigationEnd, RouteConfigLoadEnd, RouteConfigLoadStart, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { filter, map } from 'rxjs/operators';
import { InsightsService } from '@core/services/insights.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  loadingCount = 0;

  public constructor(
    private router: Router,
    private titleService: Title,
    private activatedRoute: ActivatedRoute,
    private insightsService: InsightsService,
  ) {
    this.insightsService.init();
  }

  ngOnInit(): void {
    const appTitle = this.titleService.getTitle();

    this.router.events
      .pipe(
        filter(
          event =>
            event instanceof RouteConfigLoadStart ||
            event instanceof RouteConfigLoadEnd,
        )
      )
      .subscribe(event => {
        if (event instanceof RouteConfigLoadStart) {
          this.loadingCount++;
        } else {
          this.loadingCount--;
        }
      });

    this.router.events
      .pipe(
        filter(
          event => event instanceof NavigationEnd
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
        })
      )
      .subscribe((newTitle: string) => {
        this.setTitle(newTitle);
      });
  }

  private setTitle(newTitle: string): void {
    this.titleService.setTitle(newTitle + ' | Belton Historical Society');
  }
}
