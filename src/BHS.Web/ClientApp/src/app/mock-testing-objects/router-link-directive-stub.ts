/* eslint-disable @angular-eslint/directive-class-suffix */
/* eslint-disable @angular-eslint/directive-selector */
import { Directive, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[routerLink]'
})
export class RouterLinkDirectiveStub {
  @Input() routerLink: string | unknown[] | null | undefined;
  navigatedTo: string | unknown[] | null | undefined = null;

  @HostListener('click')
  onClick(): void {
    this.navigatedTo = this.routerLink;
  }
}
