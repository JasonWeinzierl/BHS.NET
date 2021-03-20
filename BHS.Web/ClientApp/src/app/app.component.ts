import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd, ActivatedRoute, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  isDoneLoading = true;
  asyncLoadCount = 0;

  public constructor(
    private router: Router,
    private titleService: Title,
    private activatedRoute: ActivatedRoute
  ) { }

  public setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle + ' | Belton Historical Society');
  }

  ngOnInit() {
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
          this.asyncLoadCount++;
        } else if (event instanceof RouteConfigLoadEnd) {
          this.asyncLoadCount--;
        }

        this.isDoneLoading = this.asyncLoadCount <= 0;
      });

    this.router.events
      .pipe(
        filter(
          event => event instanceof NavigationEnd
        ),
        map(() => {
          let child = this.activatedRoute.firstChild;
          while (child.firstChild) {
            child = child.firstChild;
          }
          if (child.snapshot.data['title']) {
            return child.snapshot.data['title'];
          }
          return appTitle;
        })
      )
      .subscribe((newTitle: string) => {
        this.setTitle(newTitle);
      });
  }
}
