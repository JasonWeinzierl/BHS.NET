import { inject, Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRouteSnapshot, RouterStateSnapshot, TitleStrategy } from '@angular/router';

const APP_TITLE = 'Belton Historical Society';

@Injectable({
  providedIn: 'root',
})
export class BhsTitleStrategy extends TitleStrategy {
  private readonly title = inject(Title);

  override updateTitle(snapshot: RouterStateSnapshot): void {
    const title = this.buildTitle(snapshot);

    if (title !== undefined) {
      this.title.setTitle(title + ' | ' + APP_TITLE);
    } else {
      this.title.setTitle(APP_TITLE);
    }
  }

  // TODO: move title up out of data for each route and then remove this override.
  override getResolvedTitleForRoute(snapshot: ActivatedRouteSnapshot): string | undefined {
    const title: unknown = snapshot.data['title'];
    return title && typeof title === 'string' ? title : undefined;
  }
}
