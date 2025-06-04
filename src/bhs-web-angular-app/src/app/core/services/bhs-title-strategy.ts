import { inject, Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { RouterStateSnapshot, TitleStrategy } from '@angular/router';

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
}
