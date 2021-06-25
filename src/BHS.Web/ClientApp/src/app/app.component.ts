import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd, ActivatedRoute, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { IsLoadingService } from '@service-work/is-loading';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  public constructor(
    private router: Router,
    private titleService: Title,
    private activatedRoute: ActivatedRoute,
    private isLoadingService: IsLoadingService,
  ) { }

  public setTitle(newTitle: string): void {
    this.titleService.setTitle(newTitle + ' | Belton Historical Society');
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
          this.isLoadingService.add();
        } else {
          this.isLoadingService.remove();
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
}
