import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd, ActivatedRoute, NavigationStart, NavigationCancel, NavigationError } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  isDoneLoading = true;

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
            event instanceof NavigationStart ||
            event instanceof NavigationEnd ||
            event instanceof NavigationCancel ||
            event instanceof NavigationError,
        )
      )
      .subscribe(event => {
        this.isDoneLoading = !(event instanceof NavigationStart);
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
